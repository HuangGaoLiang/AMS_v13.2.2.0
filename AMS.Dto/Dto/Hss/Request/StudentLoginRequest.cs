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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.Hss
{
    /// <summary>
    /// 描述：学生登陆请求信息
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class StudentLoginRequest
    {
        /// <summary>
        /// 登陆账号--手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 手机收到的验证码
        /// </summary>
        public string SmsCode { get; set; }
        /// <summary>
        /// 用户登录凭证（有效期五分钟）。开发者需要在开发者服务器后台调用 code2Session，使用 code 换取 openid 和 session_key 等信息
        /// </summary>
        public string WxCode { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [JsonIgnore]
        public string IpAddress { get; set; }
        /// <summary>
        /// 不包括敏感信息的原始数据字符串，用于计算签名
        /// </summary>
        //public string RawData { get; set; }
        /// <summary>
        /// 使用 sha1( rawData + sessionkey ) 得到字符串，用于校验用户信息，详见 用户数据的签名验证和加解密
        /// </summary>
        //public string Signature { get; set; }
        /// <summary>
        /// 包括敏感数据在内的完整用户信息的加密数据，详见 用户数据的签名验证和加解密
        /// </summary>
        public string EncryptedData { get; set; }
        /// <summary>
        /// 加密算法的初始向量，详见 用户数据的签名验证和加解密
        /// </summary>
        public string Iv { get; set; }
    }
}
