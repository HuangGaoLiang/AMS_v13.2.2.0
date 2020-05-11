using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using YMM.HRS.SDK;
using YMM.HRS.SDK.Dto;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：补课周课表资源
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    [Route("api/AMS/ReplenishWeek")]
    [ApiController]
    public class ReplenishWeekController : BaseController
    {
        /// <summary>
        /// 补课周补课分页列表
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">查询补课周补课列表条件</param>
        /// <returns>返回补课周补课分页数据</returns>
        [HttpGet, Route("GetReplenishWeekList")]
        public PageResult<ReplenishWeekListResponse> GetReplenishWeekList([FromQuery]ReplenishWeekListSearchRequest request)
        {
            request.SchoolId = base.SchoolId;
            request.TeacherId = base.CurrentUser.UserId;
            return new ReplenishWeekService(base.SchoolId, base.CurrentUser.UserId, base.CurrentUser.UserName).GetReplenishWeekList(request);
        }


        /// <summary>
        /// 根据学生编号和班级编号获取安排的补课记录
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="classId">班级编号</param>
        /// <returns>返回安排补课信息</returns>
        [HttpGet, Route("GetClassTimeList")]
        public List<ReplenishWeekClassTimeListResponse> GetClassTimeList(long studentId, long classId)
        {
            return new ReplenishWeekService(base.SchoolId, base.CurrentUser.UserId, base.CurrentUser.UserName).GetClassTimeList(studentId, classId);
        }


        /// <summary>
        /// 补课周补课添加备注
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">备注</param>
        [HttpPost, Route("ModifyRemark")]
        public void ModifyRemark([FromBody]ReplenishWeekModifyRemarkRequest request)
        {
            new ReplenishWeekService(base.SchoolId, base.CurrentUser.UserId, base.CurrentUser.UserName).ModifyRemark(request);
        }

        /// <summary>
        /// 删除补课周补课
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="adjustLessonIds">安排补课班级的记录编号</param>
        [HttpDelete]
        public void Delete([FromBody]List<long> adjustLessonIds)
        {
            new ReplenishWeekService(base.SchoolId, base.CurrentUser.UserId, base.CurrentUser.UserName).RemoveClassTime(adjustLessonIds);
        }

        /// <summary>
        /// 添加补课周课次
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">安排补课参数信息</param>
        [HttpPost]
        public void Post([FromBody]ReplenishWeekClassTimeAddRequest request)
        {
            new ReplenishWeekService(base.SchoolId, base.CurrentUser.UserId, base.CurrentUser.UserName).AddClassTime(request);
        }
    }
}
