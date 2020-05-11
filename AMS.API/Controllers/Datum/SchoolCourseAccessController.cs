using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AMS.Service;
using AMS.Dto;
using System.Threading.Tasks;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 校区课程授权相关
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>

    [Route("api/AMS/SchoolCourseAccess")]
    [ApiController]
    public class SchoolCourseAccessController : BaseController
    {
        #region  GetList 获取校区课程授权列表

        /// <summary>
        /// 获取校区课程授权列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="cityId">城市id</param>
        /// <param name="schoolName">校区名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回课程授权集合</returns>
        [HttpGet, Route("GetList")]
        public PageResult<SchoolCourseListResponse> GetList(int cityId, string schoolName, int pageIndex, int pageSize)
        {
            return SchoolCourseService.GetList(base.CurrentUser.CompanyId, cityId, schoolName, pageIndex, pageSize);
        }

        #endregion

        #region  BatchSave 批量授权
        /// <summary>
        /// 批量授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="dto">校区课程实体</param>
        [HttpPost, Route("BatchSave")]
        public async Task BatchSaveAsync(SchoolCourseSaveBatchRequest dto)
        {
            dto.CompanyId = base.CurrentUser.CompanyId;
            await SchoolCourseService.BatchSaveAsync(dto);
        }

        #endregion

        #region  SaveAsync 单个授权
        /// <summary>
        /// 单个授权
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="schoolNo">校区编号</param>
        /// <param name="courseIds">课程编号集合</param>
        [HttpPost, Route("Save")]
        public async Task SaveAsync(string schoolNo, List<long> courseIds)
        {
            await SchoolCourseService.CreateAccess(schoolNo, base.CurrentUser.CompanyId).SaveAsync(courseIds);
        }
        #endregion

        #region  GetSchoolCourse 根据校区编号获取校区授权的课程
        /// <summary>
        /// 根据校区编号获取校区授权的课程
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="schoolNo">校区编号</param>
        /// <returns>返回校区课程集合列表</returns>
        [HttpGet, Route("GetSchoolCourse")]
        public List<SchoolCourseAllResponse> GetSchoolCourse(string schoolNo)
        {
            return SchoolCourseService.CreateAccess(schoolNo, base.CurrentUser.CompanyId).Course;
        }

        #endregion


    }
}
