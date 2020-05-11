using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：Http请求访问接口
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-09</para>
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// 执行HTTP GET请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        string Get(string url, IDictionary<string, string> parameters);

        /// <summary>
        /// 执行HTTP GET请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        T Get<T>(string url, IDictionary<string, string> parameters) where T : class;

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>HTTP响应字符串</returns>
        string Post(string url, string postData);

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>HTTP响应T实体类</returns>
        T Post<T>(string url, string postData) where T : class;

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headParameters">Head请求参数</param>
        /// <param name="bodyParamsString">Body请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        string Post(string url, IDictionary<string, string> headParameters, string bodyParamsString);

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
        T Post<T>(string url, IDictionary<string, string> headParameters, string bodyParamsString) where T : class;

        /// <summary>
        /// 执行HTTP PUT请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>HTTP响应字符串</returns>
        string Put(string url, string postData);

        /// <summary>
        /// 执行HTTP DELETE请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-09</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>0失败1成功</returns>
        string Delete(string url, IDictionary<string, string> parameters);
    }
}
