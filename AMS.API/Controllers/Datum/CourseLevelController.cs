using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 课程级别资源
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    [Route("api/AMS/CourseLevel")]
    [ApiController]
    public class CourseLevelController : BaseController
    {
        /// <summary>
        /// 获取课程级别设置列表
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <returns>返回课程级别设置列表集合</returns>
        [HttpGet]
        public async Task<List<CourseLevelResponse>> Get()
        {
            return await new CourseLevelService(base.CurrentUser.CompanyId).GetList();
        }

        /// <summary>
        /// 根据课程等级编号设置课程级别的启用状态
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <param name="courseLevelId">课程级别id</param>
        [HttpPost, Route("SetEnable")]
        public async Task SetEnable(long courseLevelId)
        {
            await new CourseLevelService(base.CurrentUser.CompanyId).SetEnable(courseLevelId);
        }

        /// <summary>
        /// 根据课程等级编号设置课程级别的禁用状态
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <param name="courseLevelId">课程级别id</param>
        [HttpPost, Route("SetDisable")]
        public async Task SetDisable(long courseLevelId)
        {
            await new CourseLevelService(base.CurrentUser.CompanyId).SetDisable(courseLevelId);
        }

        /// <summary>
        /// 增加课程级别
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <param name="request">课程等级实体</param>
        [HttpPost]
        public async Task Post([FromBody]CourseLevelAddRequest request)
        {
            await new CourseLevelService(base.CurrentUser.CompanyId).Add(request);
        }

        /// <summary>
        /// 修改课程级别
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <param name="request">课程等级实体</param>
        [HttpPut]
        public async Task Put([FromBody]CourseLevelAddRequest request)
        {
            request.CompanyId = base.CurrentUser.CompanyId;
            await new CourseLevelService(base.CurrentUser.CompanyId).Modify(request);
        }

        /// <summary>
        /// 根据课程等级编号删除课程级别
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-14 </para>
        /// </summary>
        /// <param name="courseLevelId">课程等级编号</param>
        [HttpDelete]
        public async Task Delete(long courseLevelId)
        {
            await new CourseLevelService(base.CurrentUser.CompanyId).Remove(courseLevelId);
        }
    }
}
