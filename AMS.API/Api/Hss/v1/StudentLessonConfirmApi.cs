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
using System;
using AMS.API.Api.Hss.Filters;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Hss;
using AMS.Service;
using AMS.Service.Hss;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Api.Hss.v1
{
    /// <summary>
    /// 描    述：学生考勤操作功能
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/StudentLessonConfirm")]
    [ApiController]
    public class StudentLessonConfirmApi : ControllerBase
    {
        /// <summary>
        /// 补签家长确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="replenishCode">补签码</param>
        [HttpPost, Route("ConfirmReplenishLesson")]
        public void ConfirmReplenishLesson(string replenishCode)
        {
            StudentTimetableService.ConfirmReplenishLesson(replenishCode);
        }
    }
}
