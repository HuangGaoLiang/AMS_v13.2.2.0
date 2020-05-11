using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述: 一个校区课程授权服务 
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-19 </para>
    /// </summary>
    public class SchoolCourseAccessService : BService
    {
        private readonly Lazy<TblDatCourseRepository> _courseRepository = new Lazy<TblDatCourseRepository>();
        private readonly Lazy<TblSchoolCourseRepository> _schoolCourseRepository = new Lazy<TblSchoolCourseRepository>();

        private readonly string _schoolId;  //校区
        private readonly string _companyId;  //公司编号
        private List<SchoolCourseAllResponse> _course; //校区已授权的所有课程，包括未授权的课程

        /// <summary>
        /// 初始化一个校区的课程授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-19 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="companyId">公司编号</param>
        internal SchoolCourseAccessService(string schoolId, string companyId)
        {
            _schoolId = schoolId;
            _companyId = companyId;
        }

        #region AuthCourses 获取校区授权课程
        private List<TblDatCourse> _authCourses;

        /// <summary>
        /// 校区授权的课程
        /// 作     者:zhiwei.Tang 2018.11.07
        /// </summary>
        public List<TblDatCourse> AuthCourses
        {
            get
            {
                if (_authCourses == null)
                {
                    _authCourses = GetAuthCourse();
                }
                return _authCourses;
            }
        }

        /// <summary>
        /// 获取该校区授权课程
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2018-09-15 </para>
        /// </summary>
        /// <returns>返回课程列表集合</returns>
        private List<TblDatCourse> GetAuthCourse()
        {

            List<TblDatSchoolCourse> schoolCourses = _schoolCourseRepository.Value.GetSchoolCourseListBySchoolId(_schoolId).Result;
            if (!schoolCourses.Any())
            {
                return new List<TblDatCourse>();
            }

            List<long> courseIds = schoolCourses.Select(x => x.CourseId).Distinct().ToList();

            return _courseRepository.Value.GetCoursesAsync(courseIds).Result;
        }
        #endregion

        #region Course 校区已授权的所有课程，包括未授权的课程   
        /// <summary>
        /// 校区已授权的所有课程，包括未授权的课程
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-19 </para>
        /// </summary>
        public List<SchoolCourseAllResponse> Course
        {
            get
            {
                if (_course == null)
                {
                    _course = GetSchoolCourseList(_schoolId);
                }
                return _course;
            }
        }
        #endregion

        #region GetSchoolCourseList 根据校区编号获取校区课程(区分选修和必修)
        /// <summary>
        /// 根据校区编号获取校区课程(区分选修和必修)
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-19 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        private List<SchoolCourseAllResponse> GetSchoolCourseList(string schoolId)
        {
            List<SchoolCourseAllResponse> schoolCourseAllResponse = new List<SchoolCourseAllResponse>();

            SchoolCourseAllResponse schoolCourse = new SchoolCourseAllResponse();

            // 1、获取所有的课程
            List<TblDatCourse> courseAllList = _courseRepository.Value.LoadLisTask(m => m.CompanyId == this._companyId).Result;

            // 2、获取授权的校区课程
            List<TblDatSchoolCourse> schoolCourseIds = GetDatSchoolCourseList(schoolId).Result.ToList();

            // 3、组合数据            
            List<SchoolCourseDetailResponse> list = (from c in courseAllList
                                                     join s in schoolCourseIds on c.CourseId equals s.CourseId into sc
                                                     from temp in sc.DefaultIfEmpty()
                                                     orderby c.CourseCode ascending
                                                     select new SchoolCourseDetailResponse
                                                     {
                                                         CourseId = c.CourseId,
                                                         CourseName = c.CourseCnName,
                                                         CourseType = c.CourseType,
                                                         IsDisabled = c.IsDisabled,
                                                         IsCheck = temp != null
                                                     }).ToList();

            // 4、未启用的课程不显示，如果已经授权了，则需要显示
            list = list.Where(m => m.IsCheck || (m.IsCheck == false && m.IsDisabled == false)).ToList();

            schoolCourse.RequiredCourse = list.Where(m => m.CourseType == (int)CourseType.Compulsory).ToList();
            schoolCourse.ElectiveCourse = list.Where(m => m.CourseType == (int)CourseType.Elective).ToList();
            schoolCourseAllResponse.Add(schoolCourse);
            return schoolCourseAllResponse;
        }
        /// <summary>
        /// 根据校区编号获取授权通过的课程信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-10</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回校区课程集合数据</returns>
        private async Task<List<TblDatSchoolCourse>> GetDatSchoolCourseList(string schoolId)
        {
            return await _schoolCourseRepository.Value.GetSchoolCourseListBySchoolId(schoolId);
        }

        #endregion

        #region SaveCoverAsync 覆盖保存,数据不存在时添加,存在时则忽略
        /// <summary>
        /// 覆盖保存,数据不存在时添加,存在时则忽略
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-18</para>
        /// </summary>
        /// <param name="courses">课程编号</param>
        public async Task SaveCoverAsync(List<long> courses)
        {
            List<long> courseIds = courses;

            // 1、根据校区编号和课程编号查询授权
            if (courses != null && courses.Any())
            {
                List<long> ids = _schoolCourseRepository.Value.GetSchoolCourseByWhere(_schoolId, courseIds).Result.Select(m => m.CourseId).Distinct().ToList();
                courseIds = courseIds.Except(ids).ToList();
            }
            // 2、批量添加
            List<TblDatSchoolCourse> schoolCourseList = new List<TblDatSchoolCourse>();
            if (courseIds != null)
            {
                foreach (long item in courseIds)
                {
                    TblDatSchoolCourse schoolCourse = new TblDatSchoolCourse
                    {
                        SchoolCourseId = IdGenerator.NextId(),
                        SchoolId = _schoolId,
                        CourseId = item,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    schoolCourseList.Add(schoolCourse);
                }
                await _schoolCourseRepository.Value.AddTask(schoolCourseList);
            }
        }

        #endregion

        #region SaveAsync 单个授权
        /// <summary> 
        /// 单个授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-18</para>
        /// </summary>
        /// <param name="courseIds">课程编号集合</param>
        public async Task SaveAsync(List<long> courseIds)
        {
            List<TblDatSchoolCourse> schoolCourseList = SchoolCouseDataConstructe(ref courseIds);

            // 1、添加之前先删除对应的数据
            await _schoolCourseRepository.Value.BatchDeleteBySchoolId(_schoolId);

            // 2、添加
            await _schoolCourseRepository.Value.AddTask(schoolCourseList);

            // 3、重新授权后，需要重新生成学习计划
            await StudyPlanService.AsyncSchoolCourse(_schoolId);
        }

        /// <summary>
        /// 单个校区课程授权数据构造
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-19</para>
        /// </summary>
        /// <param name="courseIds">课程编号集合</param>
        /// <returns>返回校区课程数据集合</returns>
        private List<TblDatSchoolCourse> SchoolCouseDataConstructe(ref List<long> courseIds)
        {
            List<TblDatSchoolCourse> schoolCourseList = new List<TblDatSchoolCourse>();

            // 1、查询有已经授权过的课程编号
            List<TblDatSchoolCourse> usedCorseList = _schoolCourseRepository.Value.GetSchoolCourseByWhere(_schoolId, courseIds).Result.ToList();
            schoolCourseList.AddRange(usedCorseList);

            // 2、将UpdateTime 更新到当前的时间
            foreach (TblDatSchoolCourse item in schoolCourseList)
            {
                item.UpdateTime = DateTime.Now;
            }

            // 3、求差集（过滤掉数据库里面的课程编号）
            List<long> dbCourseId = usedCorseList.Select(m => m.CourseId).Distinct().ToList();
            courseIds = courseIds.Except(dbCourseId).ToList();

            foreach (long courseId in courseIds)
            {
                TblDatSchoolCourse schoolCourse = new TblDatSchoolCourse
                {
                    CreateTime = DateTime.Now,
                    SchoolCourseId = IdGenerator.NextId(),
                    CourseId = courseId,
                    SchoolId = _schoolId,
                    UpdateTime = DateTime.Now
                };
                schoolCourseList.Add(schoolCourse);
            }

            return schoolCourseList;
        }

        #endregion
    }
}
