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
using System;
using System.Collections.Generic;
using System.Text;
namespace AMS.Service
{
    /// <summary>
    /// 描述：表示用户当事人,这里临时创建这些类,先让系统能跑起来
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    public class HssUserPrincipal
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 当前用户对应的OPENID
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 是否已经认证
        /// </summary>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// Token值
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 包含授权错误信息
        /// </summary>
        public string Msg { get; set; }
    }
}
