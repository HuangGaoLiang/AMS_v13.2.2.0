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
    /// 用户openid的相关信息
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class OpenIdResponse
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        public string Session_Key { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明。
        /// </summary>
        public string Unionid { get; set; }
        /// <summary>
        /// 错误码
        /// -1 系统繁忙，此时请开发者稍候再试
        /// 0 请求成功
        /// 40029 code 无效
        /// 45011 频率限制，每个用户每分钟100次
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
