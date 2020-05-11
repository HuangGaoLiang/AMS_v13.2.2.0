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
    /// 描    述：学生登陆认证相关资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/Login")]
    [ApiController]
    public class LoginApi : ControllerBase
    {
        /// <summary>
        /// 实例化学生登陆认证
        /// </summary>
        public LoginApi()
        {
        }

        /// <summary>
        /// 发送登陆验证码
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-02-26</para>
        /// </summary>
        /// <param name="mobile">手机号码</param>
        [HttpPost,Route("SendSignInCode")]
        public void SendSignInCode(string mobile)
        {
            new AuthenicationService().SendSignInCode(mobile);
        }


        /// <summary>
        /// 学生家长登陆
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-02-26</para>
        /// </summary>
        /// <param name="request">学生登陆请求信息</param>
        /// <returns>登陆的后的结果信息</returns>
        [HttpPost, Route("SignIn")]
        public StudentLoginResponse SignIn([FromBody]StudentLoginRequest request)
        {
            request.IpAddress = "";
            return new AuthenicationService().SignIn(request);
        }
    }
}
