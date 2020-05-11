using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Repository;
using Jerrisoft.ACS.SDK;
using Jerrisoft.Platform.Cache;
using Jerrisoft.Platform.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：区排课总表查询构造器
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-25</para>
    /// </summary>
    public class TermCourseTimetableSearchBuilder : BService
    {
        //审核班级表
        private readonly Lazy<TblAutClassRepository> _tblAutClassRepository = new Lazy<TblAutClassRepository>();

        /// <summary>
        /// 描述：实例化构造器对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        internal TermCourseTimetableSearchBuilder()
        {
        }

        /// <summary>
        /// 描述：获取排课列表的筛选条件数据
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>校区年度下的学期数据</returns>
        /// <exception cref="AMS.Core.BussinessException">
        ///异常ID：6, 异常描述:找不到授权校区
        /// </exception>
        public ClassCourseSearchSchoolResponse GetSearchData(string schoolId,string companyId)
        {

            var searchResult = new ClassCourseSearchSchoolResponse();
            //1、检查授权校区缓存是否存在
            //2、授权自已拥有的授权校区(找平台)
            //获取授权校区
            var currentSchoolRight = this.GetAuthorSchool();
            var schoolInfo = currentSchoolRight.FirstOrDefault(x => x.SchoolId.Trim() == schoolId.Trim());
            if (schoolInfo == null)//获取的校区不能为空
            {
                throw new BussinessException(ModelType.Timetable, 6);
            }
            searchResult.SchoolId = schoolInfo.SchoolId;
            searchResult.SchoolName = schoolInfo.SchoolName;

            //3、加载所有学期及年份，并对数据进行处理
            //3.1 实例化年度校区集合
            var yearAndTermList = new List<ClassCourseearchYearResponse>();
            TblDatTermRepository tblDatTermRepository = new TblDatTermRepository();
            //3.2 获取有学期的年度
            var yearList = tblDatTermRepository.GetShoolNoByTblDatTerm(schoolId).Select(x => x.Year).Distinct().OrderBy(x => x).ToList();
            //3.3 根据校区+所有的年度获取所有年度的学期
            var termList = tblDatTermRepository.GetTblDatTremList(schoolId, yearList);

            var schoolCourse = new SchoolCourseService(schoolId, companyId).GetCourseByType(null).Select(x => new ClassCourseResponse
            {
                CourseId = x.CourseId,
                ClassCnName = x.ClassCnName
            }).ToList();

            foreach (var year in yearList)
            {
                // 3.4循环获取年度所属校区
                var yearByTermList = termList.Where(x => x.SchoolId.Trim() == schoolId.Trim() && x.Year == year).Select(x => new ClassCourseSearchTermResponse
                {
                    TermId = x.TermId,
                    TermName = x.TermName,
                }).ToList();
                foreach (var termItem in yearByTermList)
                {

                    //学期下正式的所有班级  
                    var termClassList = GetTermClassCourseInfo(termItem.TermId);
                    //审核中的班级
                    var auditClassCourse = GetAuditClassCourse(termItem.TermId);
                    schoolCourse.AddRange(termClassList);
                    schoolCourse.AddRange(auditClassCourse);

                    termItem.TermCourse = schoolCourse.Distinct(new CourseInfoComparer()).ToList();
                }

                var model = new ClassCourseearchYearResponse()
                {
                    Year = year,
                    Terms = yearByTermList
                };
                yearAndTermList.Add(model);
            }
            searchResult.Years = yearAndTermList;

            //5、将处理好的数据缓存到Redis
            //this.SetCache(result);

            return searchResult;

        }
        /// <summary>
        /// 描述：获取授权校区
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <returns>授权校区列表</returns>
        /// <exception cref="AMS.Core.BussinessException">
        ///  异常ID：6, 异常描述:获取平台授权校区SDK失败
        /// </exception>
        private List<SchoolRightList> GetAuthorSchool()
        {
            try
            {
                var currentSchoolRight = this.GetFromCache();
                if (!currentSchoolRight.Any())
                {
                    SchoolRightSDK schoolRightList = new SchoolRightSDK();
                    currentSchoolRight = schoolRightList.GetSchool(CurrentUserId);
                    this.SetCache(currentSchoolRight);
                }

                return currentSchoolRight;
            }
            catch (Exception ex)
            {
                LogWriter.Write(ex, "获取平台授权校区SDK失败:" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 描述：根据学期获取审核中班级所属课程
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-17</para>
        /// </summary>
        /// <param name="termId">学期id</param>
        /// <returns>审核中的课程列表</returns>
           
        public List<ClassCourseResponse> GetAuditClassCourse(long termId)
        {
            var auditCourseList = new List<ClassCourseResponse>();
            TermCourseTimetableAuditService termCourseTiemtableService = new TermCourseTimetableAuditService(termId);
            if (termCourseTiemtableService.IsAuditing)
            {
                //所有课程
                var courseList = new TblDatCourseRepository().LoadList(x => x.IsDisabled == false);
                var classList = _tblAutClassRepository.Value.GetByAuditId(termCourseTiemtableService.TblAutAudit.AuditId).Result;

                auditCourseList = (from x1 in classList
                                   join x2 in courseList on x1.CourseId equals x2.CourseId
                                   select new ClassCourseResponse
                                   {
                                       CourseId = x1.CourseId,
                                       ClassCnName = x2.ClassCnName
                                   }).ToList();
            }

            return auditCourseList;
        }

        /// <summary>
        /// 描述：根据学期获取本学期正式班级里面的课程信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-17</para>
        /// </summary>
        /// <param name="termId">学期id</param>
        /// <returns>已通过课程信息列表</returns>
           
        public static List<ClassCourseResponse> GetTermClassCourseInfo(long termId)
        {
            var termClassList = DefaultClassService.GetClasssByTermId(termId);
            //所有课程
            var courseList = new TblDatCourseRepository().LoadList(x => x.IsDisabled == false);
            //校区所属课程

            var termClassCourseList = (from x1 in courseList
                                       join x2 in termClassList on x1.CourseId equals x2.CourseId
                                       select new ClassCourseResponse
                                       {
                                           CourseId = x1.CourseId,
                                           ClassCnName = x1.ClassCnName
                                       }).ToList();
            return termClassCourseList;
        }



        /// <summary>
        /// 描述：根据学期获取老师及所属课程
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <returns>学期下老师及所属课程列表</returns>
           
        public List<ClassCourseSearchTeacherResponse> GetTermIdByTeacherList(long termId)
        {
            TblDatClassRepository tblDatClassService = new TblDatClassRepository();

            //获取所有的老师
            var teacherList = TeachService.GetTeachers();
            TermCourseTimetableAuditService auditService = new TermCourseTimetableAuditService(termId);

            //var termTeacherIdList = new List<string>();
            var termTeacherIdList = new List<CourseInfo>();
            var courseList = new List<CourseInfo>();
            if (auditService.NoPass) //审核中
            {
                //根据学期Id查询审核中的班级信息
                TblAutClassRepository tblAutClassRepository = new TblAutClassRepository();
                var auditId = auditService.TblAutAudit.AuditId;  //获取审核中数据的审核Id
                courseList = tblAutClassRepository.GetByAuditId(auditId).Result.Select(x => new CourseInfo
                {
                    TeacherId = x.TeacherId,
                    CourseId = x.CourseId
                }).ToList();
                termTeacherIdList = courseList.Distinct(new TeacherComparer()).ToList();
            }
            else  //已生效
            {
                //根据学期Id查询已生效的班级信息
                courseList = tblDatClassService.GetTermIdByClass(termId).Select(x => new CourseInfo
                {
                    TeacherId = x.TeacherId,
                    CourseId = x.CourseId
                }).ToList();
                termTeacherIdList = courseList.Distinct(new TeacherComparer()).ToList();

            }

            var classTeachList = (from x1 in termTeacherIdList
                                  join x2 in teacherList on x1.TeacherId equals x2.TeacherId
                                  select new ClassCourseSearchTeacherResponse
                                  {
                                      TeacherNo = x2.TeacherId,
                                      TeacherName = x2.TeacherName
                                  }).ToList();
            //获取老师所属的课程
            foreach (var courseItem in classTeachList)
            {
                courseItem.TeacherByCourse = courseList.Where(x => x.TeacherId == courseItem.TeacherNo).Select(x => new ClassCourseResponse
                {
                    CourseId = x.CourseId
                }).Distinct(new CourseInfoComparer()).ToList();
            }

            return classTeachList;
        }

        /// <summary>
        /// 描述：从缓存取授权校区数据
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <returns>授权校区列表</returns>
        private List<SchoolRightList> GetFromCache()
        {
            string key = BusinessCacheKey.RightShoolDataKey;
            //从Redis缓存里面取出数据,使用平台的Redis
            var res = CacheModular.Get<List<SchoolRightList>>(base.BussinessId, key);
            //return res == null ? new List<SchoolRightList>() : res;
            return res ?? new List<SchoolRightList>();
        }

        /// <summary>
        /// 描述：保存授权校区缓存，缓存时间为10分钟
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018.9.25</para>
        /// </summary>
        /// <param name="schoolList">校区列表</param>
        private void SetCache(List<SchoolRightList> schoolList)
        {
            string key = BusinessCacheKey.RightShoolDataKey;
            CacheModular.Set(base.BussinessId, key, schoolList, TimeSpan.FromMinutes(10));
        }
    }

    /// <summary>
    /// 描述：需要比较的信息
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.9.25</para>
    /// </summary>
    public class CourseInfo
    {
        public string TeacherId { get; set; }


        public long CourseId { get; set; }
    }

    /// <summary>
    /// 描述：教师信息比较器
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.9.25</para>
    /// </summary>
    public class TeacherComparer : IEqualityComparer<CourseInfo>
    {
        public bool Equals(CourseInfo x, CourseInfo y)
        {
            if (x == null)
                return y == null;
            return x.TeacherId == y.TeacherId;
        }

        public int GetHashCode(CourseInfo obj)
        {
            if (obj == null)
                return 0;
            return obj.TeacherId.GetHashCode();
        }
    }
    /// <summary>
    /// 描述：课程信息比较器
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018.9.25</para>
    /// </summary>
    public class CourseInfoComparer : IEqualityComparer<ClassCourseResponse>
    {
        public bool Equals(ClassCourseResponse x, ClassCourseResponse y)
        {
            if (x == null)
                return y == null;
            return x.CourseId == y.CourseId;
        }

        public int GetHashCode(ClassCourseResponse obj)
        {
            if (obj == null)
                return 0;
            return obj.CourseId.GetHashCode();
        }
    }

}
