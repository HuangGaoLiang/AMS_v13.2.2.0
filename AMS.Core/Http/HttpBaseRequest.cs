using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：Http Base请求基础类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-09</para>
    /// </summary>
    public abstract class HttpBaseRequest
    {
        #region Fields & Variables & Properties
        /// <summary>
        /// 请求数据类型（默认为Json）
        /// </summary>
        public HttpContentType HttpContentType { get; set; } = HttpContentType.Json;

        /// <summary>
        /// Json的日期格式
        /// </summary>
        public JsonSerializerSettings JsonSerializer { get; set; } = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };

        /// <summary>
        /// 编码格式(默认为UTF-8)
        /// </summary>
        public virtual string Charset { get; set; } = "UTF-8";

        /// <summary>
        /// 请求超时时间（默认为100000）
        /// </summary>
        public virtual double Timeout { get; set; } = 100000;

        /// <summary>
        /// 认证令牌
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        public virtual string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.87 Safari/537.36";

        /// <summary>
        /// 接受语言
        /// </summary>
        public virtual string AcceptLanguage { get; set; }

        /// <summary>
        /// /接受编码
        /// </summary>
        public virtual string AcceptEncoding { get; set; }

        /// <summary>
        /// 内容类型
        /// </summary>
        public virtual string ContentType { get; set; }
        #endregion

        /// <summary>
        /// 地址包含Https，则需要配置支援Https请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url"></param>
        public void SetHttps(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            if (url.ToLower().StartsWith("https"))
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public virtual string GetParamters(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0) return string.Empty;
            StringBuilder result = new StringBuilder();
            foreach (var key in parameters.Keys)
            {
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(parameters[key]))
                {
                    if (result.Length > 0) result.Append("&");

                    //对value进行编码
                    string value = HttpUtility.UrlEncode(parameters[key], Encoding.GetEncoding(this.Charset));
                    result.Append(string.Join("=", key, value));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取请求内容格式
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <returns></returns>
        public virtual string GetContentType()
        {
            string contentType = string.Empty;
            switch (this.HttpContentType)
            {
                case HttpContentType.Json:
                    contentType = "application/json;charset=";
                    break;
                case HttpContentType.Form:
                    contentType = "application/x-www-form-urlencoded;charset=";
                    break;
                case HttpContentType.Stream:
                    contentType = "application/octet-stream;charset=";
                    break;
                default:
                    contentType = "application/json;charset=";
                    break;
            }
            return contentType + this.Charset;
        }
    }
}
