using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描    述：Http请求辅助类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-22</para>
    /// </summary>
    public class HttpClientRequest : HttpBaseRequest, IHttpRequest
    {
        #region Fields
        /// <summary>
        /// Http请求对象
        /// //只需要实例化一次，就可以保持多次请求
        /// </summary>
        private HttpClient _client;
        #endregion

        /// <summary>
        /// Http请求辅助类实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-22</para>
        /// </summary>
        /// <param name="httpContentType">Http内容数据类型</param>
        public HttpClientRequest(HttpContentType httpContentType = HttpContentType.Json)
        {
            base.HttpContentType = httpContentType;

            //初始化Http请求端
            InitHttpClient();
        }

        /// <summary>
        /// Http请求辅助类析构函数
        /// </summary>
        ~HttpClientRequest()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        /// <summary>
        /// Http请求端初始化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="token">令牌</param>
        public void InitHttpClient()
        {
            HttpClientHandler httpHandler = new HttpClientHandler() { UseCookies = true, UseProxy = false };//请求处理对象
            _client = new HttpClient(httpHandler);
            _client.DefaultRequestHeaders.Add("Connection", "keep-alive");

            //this.Token = token;//设置令牌
        }

        #region Http Properties
        /// <summary>
        /// 设置令牌
        /// </summary>
        public override string Token
        {
            get => base.Token;
            set
            {
                base.Token = value;
                _client.ChangeRequestHeader("SSO-TOKEN", base.Token);
            }
        }

        /// <summary>
        /// //设置请求时间
        /// </summary>
        public override double Timeout
        {
            get { return base.Timeout; }
            set
            {
                base.Timeout = value;
                _client.Timeout = TimeSpan.FromMilliseconds(base.Timeout);
            }
        }

        /// <summary>
        /// 编码格式(默认为UTF-8)
        /// </summary>
        public override string Charset
        {
            get { return base.Charset; }
            set
            {
                base.Charset = value;
                _client.ChangeRequestHeader("charset", base.Charset);
            }
        }
        #endregion

        #region Http Methods

        /// <summary>
        /// 执行HTTP GET请求。
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Get(string url, IDictionary<string, string> parameters)
        {
            string result = string.Empty;
            HttpResponseMessage response = null;
            Stream stream = null;
            try
            {
                //添加网址参数
                if (parameters != null && parameters.Count > 0)
                {
                    url = url + (url.EndsWith("?") ? "&" : "?") + GetParamters(parameters);
                }
                //发送POST请求
                response = _client.GetAsync(url).Result;
                //请求成功返回结果 
                if (response.IsSuccessStatusCode)
                {
                    stream = response.Content.ReadAsStreamAsync().Result;
                    using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(this.Charset)))
                    {
                        stream = null;
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Http Request Error Message: ", ex);
            }
            finally
            {
                //释放response对象
                if (response != null)
                {
                    response.Dispose();
                }
                //释放stream对象
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Get<T>(string url, IDictionary<string, string> parameters) where T : class
        {
            return GetResponse<T>(Get(url, parameters));
        }

        /// <summary>
        /// 将Http Response返回的结果实例成实体
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <typeparam name="T">返回结果实体</typeparam>
        /// <param name="responseResult">请求返回的字符串</param>
        /// <returns>HTTP响应T实体类</returns>
        private T GetResponse<T>(string responseResult)
        {
            var result = JsonConvert.DeserializeObject<T>(responseResult);
            if (result == null)
            {
                return default(T);
            }
            return result;
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>HTTP响应字符串</returns>
        public string Post(string url, string postData)
        {
            string result = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                //发送POST请求
                using (HttpContent httpContent = GetHttpContent(postData))
                {
                    response = _client.PostAsync(url, httpContent).Result;
                }
                //请求成功返回结果 
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Http Request Error Message: ", ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Post<T>(string url, string postData) where T : class
        {
            return GetResponse<T>(Post(url, postData));
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headParameters">Head请求参数</param>
        /// <param name="bodyParamsString">Body请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Post(string url, IDictionary<string, string> headParameters, string bodyParamsString)
        {
            string result = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                //存在请求头部有参数，则添加至Http Request Header
                if (headParameters != null && headParameters.Count > 0)
                {
                    foreach (var param in headParameters.Keys)
                    {
                        _client.ChangeRequestHeader(param, headParameters[param]);
                    }
                }
                //发送POST请求
                using (HttpContent httpContent = GetHttpContent(bodyParamsString))
                {
                    response = _client.PostAsync(url, httpContent).Result;
                }
                //请求成功返回结果 
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Http Request Error Message: ", ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <typeparam name="T">返回T实体类</typeparam>
        /// <param name="url">请求路径</param>
        /// <param name="headParameters">表头请求参数</param>
        /// <param name="bodyParamString">表体请求参数</param>
        /// <returns>HTTP响应T实体类</returns>
        public T Post<T>(string url, IDictionary<string, string> headParameters, string bodyParamsString) where T : class
        {
            return JsonConvert.DeserializeObject<T>(Post(url, headParameters, bodyParamsString), base.JsonSerializer);
        }

        /// <summary>
        /// 执行HTTP PUT请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="postData">请求参数</param>
        /// <returns>HTTP响应字符串</returns>
        public string Put(string url, string postData)
        {
            string result = string.Empty;
            HttpResponseMessage response = null;
            try
            {
                //发送PUT请求
                using (HttpContent httpContent = GetHttpContent(postData))
                {
                    response = _client.PutAsync(url, httpContent).Result;
                }
                //请求成功返回结果 
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Http Request Error Message: ", ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 执行HTTP DELETE请求
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>0失败1成功</returns>
        public string Delete(string url, IDictionary<string, string> parameters)
        {
            try
            {
                //添加网址参数
                if (parameters != null && parameters.Count > 0)
                {
                    url = url + (url.EndsWith("?") ? "&" : "?") + GetParamters(parameters);
                }
                //发送DELETE请求
                using (HttpResponseMessage response = _client.DeleteAsync(url).Result)
                {
                    //请求成功返回结果 
                    if (response.IsSuccessStatusCode)
                    {
                        return "1";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Http Request Error Message: ", ex);
            }
            return "0";
        }

        #endregion

        /// <summary>
        /// 获取Http请求体
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <param name="postData">请求参数字符串</param>
        /// <returns>Http内容</returns>
        private HttpContent GetHttpContent(string postData)
        {
            HttpContent httpContent = new StringContent(postData);
            httpContent.ChangeContentHeader("Content-Type", base.GetContentType());
            return httpContent;
        }
    }
}
