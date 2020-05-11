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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.API.Api.Hss.Filters
{
    /// <summary>
    /// 描述：学生用户认证基类,这里临时创建这些类,先让系统能跑起来
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public abstract class BaseAuthorizeAttribute : Attribute,IActionFilter
    {
        /// <summary>
        /// 控制器执行之后
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-02-26</para>
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext.</param>
        public abstract void OnActionExecuted(ActionExecutedContext context);

        /// <summary>
        /// 控制器执行之前
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-02-26</para>
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext.</param>
        public abstract void OnActionExecuting(ActionExecutingContext context);



    }
}
