/****************************************************************************\
所属系统:招生系统
所属模块:微信授权服务
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

namespace AMS.Anticorrosion.WX
{
    /// <summary>
    /// 与微信授权服务交互的相关地址
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class WxConfig
    {

        protected WxConfig() { }
        /// <summary>
        /// 授权类型，此处只需填写 authorization_code
        /// </summary>
        public const string GRANT_TYPE = "authorization_code";
        /// <summary>
        /// 表示返回成功的结果
        /// </summary>
        public const int RESULT_SUCCESS = 0;
    }
}
