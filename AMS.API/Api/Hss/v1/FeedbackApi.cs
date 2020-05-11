/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.API.Api.Hss.Filters;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Api.Hss.v1
{
    /// <summary>
    /// 描    述：意见和反馈
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/Feedback")]
    [ApiController]
    public class FeedbackApi : BaseStudentApi
    {
        /// <summary>
        /// 提交反馈
        /// <para>作    者： 郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="feedbackRequest">回馈添加请求对象</param>
        [HttpPost]
        public async Task Post(FeedbackAddRequest feedbackRequest)
        {
            await new FeedbackService(feedbackRequest.CompanyId).AddIFeedback(feedbackRequest);
        }
    }
}
