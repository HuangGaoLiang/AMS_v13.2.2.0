/****************************************************************************\
所属系统:招生系统
所属模块:家校互联--学生认证
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.API.Api.Hss.Filters;
using AMS.Service;
using AMS.Service.Hss;
using Jerrisoft.Platform.IdentityClient.Configs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.API.Api.Hss.Filters
{
    /// <summary>
    /// 描述：JWT TOKEN处理
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public class JwtHandler
    {
        private const string TOKEN_KEY = "HSS-TOKEN";


        /// <summary>
        /// 处理TOKEN成用户信息
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns></returns>
        public HssUserPrincipal Handler(HttpContext content)
        {
            HssUserPrincipal user = null;
            string token = TokenProvider.GetToken(content, TOKEN_KEY);
            JwtTokenService tokenService = new JwtTokenService();
            user = tokenService.GetUser(token);
            if (user != null)
            {
                user.IsAuthenticated = true;
            }
            else
            {
                user = new HssUserPrincipal();
                user.IsAuthenticated = false;
                user.Msg = ErrorMsgConfig.ERROR_TOKEN_DESCRYPTION_FAILED;
            }
            return user;
        }

    }
}
