using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 课程资源
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Course")]
    [ApiController]
    public class CourseController : BaseController
    {
        /// <summary>
        /// 获取课程列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseType">1必修课 2选修课</param>
        /// <returns>课程列表</returns>
        [HttpGet]
        public List<CourseListResponse> Get(CourseType courseType)
        {
            return new CourseService(base.CurrentUser.CompanyId).GetList((int)courseType);
        }

        /// <summary>
        /// 获取课程简要信息列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="cType">0:必修课程 1:选修课程 null:所有</param>
        /// <returns>课程简要信息列表</returns>
        [HttpGet, Route("CourseShort")]
        public List<CourseShortResponse> GetShortList(int? cType)
        {
            return new CourseService(base.CurrentUser.CompanyId).GetShortList(cType);
        }

        /// <summary>
        /// 获取课程详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns>课程详情</returns>
        [HttpGet, Route("Details")]
        public async Task<CourseResponse> Get(long courseId)
        {
            return await new CourseService(base.CurrentUser.CompanyId).GetCourseDetailsAsync(courseId);
        }

        /// <summary>
        /// 启用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        [HttpPost, Route("SetEnable")]
        public async Task SetEnable(long courseId)
        {
            await new CourseService(base.CurrentUser.CompanyId).SetEnableAsync(courseId);
        }

        /// <summary>
        /// 禁用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        [HttpPost, Route("SetDisable")]
        public async Task SetDisable(long courseId)
        {
            await new CourseService(base.CurrentUser.CompanyId).SetDisableAsync(courseId);
        }

        /// <summary>
        /// 删除课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="courseId">课程Id</param>
        [HttpDelete]
        public async Task Delete(long courseId)
        {
            await new CourseService(base.CurrentUser.CompanyId).RemoveAsync(courseId);
        }

        /// <summary>
        /// 新增课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-11</para>
        /// </summary>
        /// <param name="request">新增课程Dto</param>
        [HttpPost]
        public void Post([FromBody]CourseAddRequest request)
        {
            new CourseService(base.CurrentUser.CompanyId).Add(request);
        }

        /// <summary>
        /// 修改课程
        /// <para>作    者：zhiwei.Tang </para>
        /// <para>创建时间：2018-09-11 </para>
        /// </summary>
        /// <param name="courseId">课程Id </param>
        /// <param name="request">修改课程Dto </param>
        [HttpPut]
        public async Task Put(long courseId, [FromBody]CourseAddRequest request)
        {
            request.CourseId = courseId;
            request.CompanyId = base.CurrentUser.CompanyId;
            await new CourseService(base.CurrentUser.CompanyId).ModifyAsync(request);
        }

        /// <summary>
        /// 获取已经授权课程简要信息列表
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间: 2018-10-10 </para>
        /// </summary>
        /// <param name="cType">0:必修课程 1:选修课程 null:所有</param>
        /// <returns>返回已经授权课程的简要信息列表</returns>
        [HttpGet, Route("GetSchoolCourse")]
        [SchoolIdValidator]
        public async Task<List<CourseShortResponse>> GetSchoolCourse(int? cType)
        {
            return new SchoolCourseService(base.SchoolId, base.CurrentUser.CompanyId).GetCourseByType(cType);
        }

        /// <summary>
        /// 获取包含课程和等级的课程信息 必修课
        /// <para>作     者：Huang GaoLiang  </para>
        /// <para>创建时间: 2018-11-07 </para>
        /// </summary>
        /// <returns>返回课程集合</returns>
        [HttpGet, Route("GetCompulsoryCourseAndLevel")]
        [SchoolIdValidator]
        public List<CourseAndLevelResponse> GetCompulsoryCourseAndLevel()
        {
            return new SchoolCourseService(base.SchoolId, base.CurrentUser.CompanyId).GetCompulsoryCourseAndLevel(base.CurrentUser.CompanyId);
        }

    }
}
