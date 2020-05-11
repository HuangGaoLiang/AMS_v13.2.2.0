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
    /// 描    述：学生登陆TOKEN刷新
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/TokenRefresh")]
    [ApiController]
    public class TokenRefreshApi : BaseStudentApi
    {
        private readonly HttpContext _context;


        /// <summary>
        /// 校验并刷新token
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns>登陆的后的结果信息</returns>
        [HttpPut]
        public StudentLoginResponse Put()
        {
            string ipAddress = string.Empty;
            if (_context != null)
            {
                ipAddress = _context.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                ipAddress = "";
            }
            return new AuthenicationService().RefreshToken(base.UserPrincipal, ipAddress);
        }
    }
}
