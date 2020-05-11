/****************************************************************************\
所属系统:招生系统
所属模块:公共库
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

namespace AMS.Core
{
    /// <summary>
    /// 描    述：base64加密解密
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-16</para>
    /// </summary>
    public class Base64Helper
    {
        /// <summary>
        /// 构造函数,不允许直接实例化.
        /// </summary>
        protected Base64Helper()
        {

        }

        #region Base64位加密解密
        /// <summary>
        /// 将字符串转换成base64格式,使用UTF8字符集
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-16</para>
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <returns></returns>
        public static string Base64Encode(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 将base64格式，转换utf8
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-16</para>
        /// </summary>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public static string Base64Decode(string content)
        {
            byte[] bytes = Convert.FromBase64String(content);
            return Encoding.UTF8.GetString(bytes);
        }
        #endregion

    }
}
