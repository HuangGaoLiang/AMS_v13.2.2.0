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
using Jerrisoft.Platform.Exception;
using Jerrisoft.Platform.IdentityClient.Configs;
using Jerrisoft.Platform.IdentityClient.Models;
using Jerrisoft.Platform.IdentityClient.Util;
using Jerrisoft.Platform.Public.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Api.Hss.Filters
{
    /// <summary>
    /// 描述：授权服务通用类
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public class AuthenticationService
    {

        private readonly HttpContext _context;


        /// <summary>
        /// 构造函数
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="context">上下文数据</param>
        public AuthenticationService(HttpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 验证用户
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns></returns>
        public void ValidateDefaultUser()
        {
            AMS.Service.HssUserPrincipal user = GetDefaultUser();
            //用户为空或者未授权
            if (user == null || !user.IsAuthenticated)
            {
                throw new PlatformException(ErrorMsgConfig.ERROR_TOKEN_DESCRYPTION_FAILED);
            }
        }


        /// <summary>
        /// 获取默认用户
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <returns></returns>
        public AMS.Service.HssUserPrincipal GetDefaultUser()
        {
            JwtHandler handler = new JwtHandler();
            AMS.Service.HssUserPrincipal user = handler.Handler(this._context);
            return user;
        }
    }
}
