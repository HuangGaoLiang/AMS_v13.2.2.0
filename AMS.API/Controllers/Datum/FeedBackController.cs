/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Datum
{
    /// <summary>
    /// 描    述：信息投诉与建议控制器
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-05</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/FeedBack")]
    [ApiController]
    public class FeedBackController : BaseController
    {
        /// <summary>
        /// 根据查询条件获取投诉与建议信息
        /// <para>作    者：Hunag GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="searchRequest">投诉与建议查询条件</param>
        /// <returns>信息投诉与建议信息分页列表</returns>
        [HttpGet]
        public PageResult<FeedbackListResponse> Get([FromQuery]FeedbackSearchRequest searchRequest)
        {
            return new FeedbackService(base.CurrentUser.CompanyId).GetFeedBackList(searchRequest);
        }

        /// <summary>
        /// 根据学生Id获取对应的投诉与建议详情
        /// <para>作    者：Hunag GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="detailId">投诉与建议Id</param>
        /// <returns>信息投诉与建议详情</returns>
        [HttpGet, Route("GetFeedBackDetail")]
        public async Task<FeedbackDetailResponse> GetFeedBackDetail(long detailId)
        {
            return await new FeedbackService(base.CurrentUser.CompanyId).GetFeedbackDetail(detailId);
        }

        /// <summary>
        /// 更新信息投诉与建议的处理状态
        /// <para>作    者：Hunag GaoLiang </para>
        /// <para>创建时间：2019-03-07 </para>
        /// </summary>
        /// <param name="feedbackId">投诉与建议Id</param>
        /// <param name="feedbackRequest">处理信息</param>
        [HttpPut]
        public async Task Put(long feedbackId, [FromBody]FeedbackRequest feedbackRequest)
        {
            feedbackRequest.ProcessUserId = base.CurrentUser.UserId;
            feedbackRequest.ProcessUserName = base.CurrentUser.UserName;
            await new FeedbackService(base.CurrentUser.CompanyId).UpdateProcessStatus(feedbackId, feedbackRequest);
        }
    }
}
