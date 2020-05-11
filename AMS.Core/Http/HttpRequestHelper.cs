using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：HTTP请求帮助类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-09</para>
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// Http Request接口实体类
        /// </summary>
        private readonly IHttpRequest _httpRequest;

        /// <summary>
        /// HTTP请求帮忙类实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="httpRequest">Http请求类</param>
        public HttpRequestHelper(IHttpRequest httpRequest)
        {
            this._httpRequest = httpRequest;
        }

        /// <summary>
        /// 执行HTTP GET请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Get(string url, IDictionary<string, string> parameters)
        {
            return this._httpRequest.Get(url, parameters);
        }

        /// <summary>
        /// 执行HTTP GET请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Get<T>(string url, IDictionary<string, string> parameters) where T : class
        {
            return this._httpRequest.Get<T>(url, parameters);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Post(string url, string postData)
        {
            return this._httpRequest.Post(url, postData);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Post<T>(string url, string postData) where T : class
        {
            return this._httpRequest.Post<T>(url, postData);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headParameters">Head请求参数</param>
        /// <param name="bodyParamString">Body请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Post(string url, IDictionary<string, string> headParameters, string bodyParamsString)
        {
            return this._httpRequest.Post(url, headParameters, bodyParamsString);
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <typeparam name="T">返回T实体类</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="headParameters">表头请求参数</param>
        /// <param name="bodyParamsString">表体请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Post<T>(string url, IDictionary<string, string> headParameters, string bodyParamsString) where T : class
        {
            return this._httpRequest.Post<T>(url, headParameters, bodyParamsString);
        }

        /// <summary>
        /// 执行HTTP PUT请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="postData">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Put(string url, string postData)
        {
            return this._httpRequest.Put(url, postData);
        }

        /// <summary>
        /// 执行HTTP DELETE请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>0失败1成功</returns>
        public string Delete(string url, IDictionary<string, string> parameters)
        {
            return this._httpRequest.Delete(url, parameters);
        }
    }
}
