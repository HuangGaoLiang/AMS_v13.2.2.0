using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 校区学期相关基础下拉框数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    [Route("api/AMS/SchoolTermData")]
    public class SchoolTermDataController : BsnoController
    {
        /// <summary>
        /// 获取年份数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>年份基础数据</returns>
        [HttpGet, Route("GetYears")]
        public List<int> GetYears()
        {
            return new SchoolTermDataService(base.SchoolId).GetYears();
        }

        /// <summary>
        /// 获取学期类型
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>学期类型列表</returns>
        [HttpGet, Route("GetTermTypes")]
        public List<TermTypeMiniDataResponse> GetTermTypes()
        {
            return new SchoolTermDataService(base.SchoolId).GetTermTypes();
        }

        /// <summary>
        /// 根据学期类型Id加年度获取学期
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>学期列表信息</returns>
        [HttpGet, Route("GetTermByTermTypeAndYear")]
        public List<TermMiniDataResponse> GetTermByTermTypeAndYear(long termTypeId, int year)
        {
            return new SchoolTermDataService(base.SchoolId).GetTermByTermTypeAndYear(termTypeId, year);
        }

        /// <summary>
        /// 获取该校区所有授权课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>授权课程列表</returns>
        [HttpGet, Route("GetAuthCourse")]
        public List<CourseMiniDataResponse> GetAuthCourse()
        {
            return new SchoolTermDataService(base.SchoolId).GetAuthCourse(base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 根据课程ID获取课程级别
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>课程等级列表</returns>
        [HttpGet, Route("GetCourseLevel")]
        public List<CourseLevelMiniDataResponse> GetCourseLevel(long courseId)
        {
            return new SchoolTermDataService(base.SchoolId).GetCourseLevel(base.CurrentUser.CompanyId,courseId);
        }

        /// <summary>
        /// 获取教室
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <returns>教室列表</returns>
        [HttpGet, Route("GetClassRoom")]
        public List<ClassRoomMiniDataResponse> GetClassRoom()
        {
            return new SchoolTermDataService(base.SchoolId).GetClassRoom();
        }
    }
}
