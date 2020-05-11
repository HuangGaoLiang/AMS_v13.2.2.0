using AMS.Core;
using Jerrisoft.Platform.IdentityClient.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AMS.API.Controllers
{

    /*---------------------------------------------------------------------------------------------------
    * 项目名称 ：$Projectname$
    * 项目描述 ：
    * 类 名 称 ：BaseController
    * 类 描 述 ：
    * 所在的域 ：YMM-杰人-技术总
    * 命名空间 ：AMS.API
    * 机器名称 ：YMM-杰人-技术总 
    * CLR 版本 ：4.0.30319.42000
    * 作    者 ：user
    * 创建时间 ：2018/8/2 20:39:20 
    * Ver         变更日期                       负责人          变更内容
    * ─────────────────────────────────────────────────
    * V1.0.0.0    2018/8/2 20:39:20       user

    *******************************************************************

    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘

    *******************************************************************
    --------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 内部控制器调用基类,包含控制器的认证
    /// </remarks>
    [SsoAuthorize]
    public class BaseController : Jerrisoft.Platform.API.BaseController
    {
        /// <summary>
        /// 业务编号
        /// </summary>
        public override byte BussinessId => BusinessConfig.BussinessID;

        /// <summary>
        /// 校区编号
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-27</para>
        /// </summary>
        protected virtual string SchoolId
        {
            get
            {
                if (HttpContext.Request.Headers.TryGetValue("schoolNo", out StringValues schoolNo))
                {
                    return schoolNo;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 年度
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-27</para>
        /// </summary>
        protected virtual int Year
        {
            get
            {
                if (HttpContext.Request.Headers.TryGetValue("year", out StringValues year))
                {
                    return int.Parse(year);
                }
                return 0;
            }
        }
    }
}
