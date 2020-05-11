using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMS.Core
{
    /// <summary>
    ///  描    述：Http扩展类
    ///  <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-22</para>
    /// </summary>
    public static class HttpExtentions
    {
        /// <summary>
        /// 修改HttpClient的Headers配置项
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-22</para>
        /// </summary>
        /// <param name="httpClient">httpClient对象</param>
        /// <param name="name">属性</param>
        /// <param name="value">属性值</param>
        public static void ChangeRequestHeader(this HttpClient httpClient, string name, string value)
        {
            httpClient.DefaultRequestHeaders.Remove(name);
            httpClient.DefaultRequestHeaders.Add(name, value);
        }

        /// <summary>
        /// 修改HttpContent的配置项
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-22</para>
        /// </summary>
        /// <param name="httpContent">HttpContent对象</param>
        /// <param name="name">属性</param>
        /// <param name="value">属性值</param>
        public static void ChangeContentHeader(this HttpContent httpContent, string name, string value)
        {
            httpContent.Headers.Remove(name);
            httpContent.Headers.Add(name, value);
        }
    }
}
