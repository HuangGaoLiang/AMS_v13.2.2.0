using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：校区所有课程数据服务
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public class SchoolCourseService : BService
    {
        private readonly Lazy<TblDatCourseRepository> _courseRepository = new Lazy<TblDatCourseRepository>();
        private readonly Lazy<TblDatCourseLevelMiddleRepository> _datCourseLevelMiddleRepository = new Lazy<TblDatCourseLevelMiddleRepository>();
        private readonly Lazy<TblSchoolCourseRepository> _schoolCourseRepository = new Lazy<TblSchoolCourseRepository>();

        /// <summary>
        /// 校区
        /// </summary>
        private readonly string _schoolId;

        /// <summary>
        /// 公司编号
        /// </summary>
        private readonly string _companyId;



        /// <summary>
        /// 实例化一个校区的课程服务
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="companyId">公司编号</param>
        public SchoolCourseService(string schoolId, string companyId)
        {
            this._schoolId = schoolId;
            this._companyId = companyId;
        }

        #region CreateAccess 创建一个授权校区
        /// <summary>
        /// 创建一个授权校区
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回校区授权课程信息</returns>
        public static SchoolCourseAccessService CreateAccess(string schoolId, string companyId)
        {
            SchoolCourseAccessService service = new SchoolCourseAccessService(schoolId, companyId);
            return service;
        }

        #endregion

        #region BatchSaveAsync 校区批量授权课程实体
        /// <summary>
        /// 批量增加授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="dto">校区批量授权课程数据</param>
        public static async Task BatchSaveAsync(SchoolCourseSaveBatchRequest dto)
        {
            var schools = dto.Schools;

            // 1、获取所有校区编号
            foreach (string schoolId in schools)
            {
                SchoolCourseAccessService service = new SchoolCourseAccessService(schoolId, dto.CompanyId);

                // 处理后数据
                var course = dto.Course;
                await service.SaveCoverAsync(course);

                // 重新授权后，需要重新生成学习计划
                await StudyPlanService.AsyncSchoolCourse(schoolId);
            }
        }
        #endregion

        #region GetList 获取校区课程授权分页列表
        /// <summary>
        /// 获取校区课程授权分页列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市id</param>
        /// <param name="schoolName">校区名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回课程授权集合</returns>
        public static PageResult<SchoolCourseListResponse> GetList(string companyId, int cityId, string schoolName, int pageIndex, int pageSize)
        {
            // 1、获取所有的校区和城市
            List<SchoolResponse> schoolList = GetSchoolCourse(companyId, cityId, schoolName);

            // 2、获取已授权的校区编号
            List<string> schools = new TblSchoolCourseRepository().GetSchools();

            foreach (SchoolResponse item in schoolList)
            {
                if (schools.Contains(item.SchoolId))
                {
                    item.Sort = true;
                }
            }
            schoolList = schoolList.OrderBy(m => m.Sort).ThenBy(m => m.SchoolName).ToList();

            // 3、校区-城市 数据组合
            List<SchoolCourseListResponse> resultList = (from a in schoolList
                                                         select new SchoolCourseListResponse
                                                         {
                                                             SchoolId = a.SchoolId,
                                                             SchoolName = a.SchoolName,
                                                             CityId = a.CityId,
                                                             CityName = a.CityName
                                                         })
                                                         .Skip((pageIndex - 1) * pageSize)
                                                         .Take(pageSize)
                                                         .ToList();

            // 4、获取授权校区下的课程信息
            List<TblDatSchoolCourse> schoolCourseList = new TblSchoolCourseRepository().GetAllSchoolCourseList().Result;

            // 5、获取所有的课程数据
            List<TblDatCourse> courseList = CourseService.GetAllAsync().Result.Where(m => m.CompanyId == companyId).Select(m => new TblDatCourse
            {
                CourseId = m.CourseId,
                CourseCnName = m.CourseCnName,
                CourseType = m.CourseType
            }).ToList();

            // 6、校区-城市-课程组合
            foreach (SchoolCourseListResponse item in resultList)
            {
                List<long> courseIds = schoolCourseList.Where(t => string.CompareOrdinal(t.SchoolId, item.SchoolId) == 0).Select(m => m.CourseId).Distinct().ToList();
                item.RequiredCourse = GetCourseNameList(courseIds, courseList, (int)CourseType.Compulsory);
                item.ElectiveCourse = GetCourseNameList(courseIds, courseList, (int)CourseType.Elective);
            }

            PageResult<SchoolCourseListResponse> response = new PageResult<SchoolCourseListResponse>();

            response.TotalData = schoolList.Count;
            response.CurrentPage = pageIndex;
            response.PageSize = pageSize;
            response.Data = resultList;
            return response;
        }

        /// <summary>
        /// 获取必修可和选修课课程名称
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="courseIds">授权校区课程编号集合</param>
        /// <param name="courseList">所有课程</param>
        /// <param name="courseType">0：选修，1：必修</param>
        /// <returns>返回课程名称</returns>
        private static List<string> GetCourseNameList(List<long> courseIds, List<TblDatCourse> courseList, int courseType)
        {
            List<string> courseNameList = courseList.Where(m => m.CourseType == courseType && courseIds.Contains(m.CourseId))
                .OrderBy(m => m.CourseCode)
                .Select(m => m.CourseCnName)
                .ToList();
            return courseNameList;
        }

        /// <summary>
        /// 根据城市编号和校区名称查询课程授权列表信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-18 </para>
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="cityId">城市id</param>
        /// <param name="schoolName">校区名称</param>
        /// <returns></returns>
        private static List<SchoolResponse> GetSchoolCourse(string companyId, int cityId, string schoolName)
        {
            List<SchoolResponse> schoolList = new OrgService().GetAllSchoolList().Where(m => m.CompanyId == companyId).ToList();
            if (cityId > 0)
            {
                schoolList = schoolList.Where(m => m.CityId == cityId).ToList();
            }
            if (!string.IsNullOrWhiteSpace(schoolName))
            {
                schoolList = schoolList.Where(m => m.SchoolName.Contains(schoolName)).ToList();
            }

            return schoolList;
        }

        #endregion

        #region CourseIsUse 校区是否引用了课程
        /// <summary>
        /// 校区是否引用了课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018.09.15</para> 
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        public static bool CourseIsUse(long courseId)
        {
            return new TblSchoolCourseRepository().CourseIsUse(courseId).Result;
        }
        #endregion

        #region GetCourseByType 获取已经授权课程简要信息列表
        /// <summary>
        /// 获取已经授权课程简要信息列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-10 </para>
        /// </summary>
        /// <param name="cType">课程类型0:必修课程 1:选修课程 null:所有</param>
        /// <returns>返回课程信息集合</returns>
        public List<CourseShortResponse> GetCourseByType(int? cType)
        {
            // 1、获取当前校区授权的课程编号
            List<long> schoolCourseIds = GetDatSchoolCourseList(this._schoolId).Result.Select(m => m.CourseId).Distinct().ToList();

            // 2、获取授权的课程编号查询课程信息
            List<TblDatCourse> datCourseList = _courseRepository.Value.GetCourseListBycourseIds(schoolCourseIds).Where(m => m.CompanyId == this._companyId).ToList();

            // 3、根据条件查询需要的数据
            if (cType.HasValue)
            {
                datCourseList = datCourseList.Where(m => m.CourseType == cType).ToList();
            }
            // 4、按照课程代码升序排序数据
            datCourseList = datCourseList.OrderBy(m => m.CourseCode, new NaturalStringComparer()).ToList();
            List<CourseShortResponse> res = AutoMapper.Mapper.Map<List<CourseShortResponse>>(datCourseList);

            return res;
        }

        /// <summary>
        /// 根据校区编号获取授权通过的课程信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-10 </para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        /// <returns>返回课程信息集合</returns>
        private async Task<List<TblDatSchoolCourse>> GetDatSchoolCourseList(string schoolId)
        {
            return await _schoolCourseRepository.Value.GetSchoolCourseListBySchoolId(schoolId);
        }

        #endregion

        #region GetCompulsoryCourseAndLevel 获取包含课程和等级的课程信息
        /// <summary>
        /// 获取包含课程和等级的课程信息 必修课
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-07 </para>
        /// </summary>
        /// <returns>返回必修课的课程信息</returns>
        public List<CourseAndLevelResponse> GetCompulsoryCourseAndLevel(string companyId)
        {
            // 1、获取该校区所有授权通过的课程编号
            List<long> courseSchoolIds = _schoolCourseRepository.Value.LoadList(m => m.SchoolId == _schoolId).Select(m => m.CourseId).Distinct().ToList();

            // 2、根据课程编号获取课程所有必修课的课程信息
            List<TblDatCourse> courseList = CourseService.GetAllAsync().Result.Where(m => courseSchoolIds.Contains(m.CourseId) && m.CourseType == (int)CourseType.Compulsory).ToList();

            // 3、根据课程编号查询课程等级中间表中的相关等级编号
            List<long> courseIds = courseList.Select(m => m.CourseId).Distinct().ToList();
            List<TblDatCourseLevelMiddle> courseLeaveId = _datCourseLevelMiddleRepository.Value.LoadList(m => courseIds.Contains(m.CourseId)).ToList();

            // 4、获取课程对应的等级信息
            List<TblDatCourseLevel> courseLeaves = new CourseLevelService(companyId).GetList().Result.Select(m => new TblDatCourseLevel
            {
                CourseLevelId = m.CourseLevelId,
                LevelCnName = m.LevelCnName
            }).ToList();

            // 5、组合数据
            var result = (from c in courseList
                          join m in courseLeaveId on c.CourseId equals m.CourseId
                          join l in courseLeaves on m.CourseLevelId equals l.CourseLevelId
                          select new CourseAndLevelResponse
                          {
                              CourseId = c.CourseId,
                              CourseType = c.CourseType,
                              ShortName = c.ShortName,
                              CourseLeaveId = l.CourseLevelId,
                              CourseLeaveName = l.LevelCnName
                          }).ToList();

            return result;
        }
        #endregion
    }
}