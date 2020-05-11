using System.Collections.Specialized;

namespace AMS.Core
{
    public static class HttpRun
    {
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <returns></returns>
        public static string Get(string strUrl)
        {
            HttpCommand Webcommand = new HttpCommand(strUrl, "Get");
            return Webcommand.Execute();
        }
        public static string Get(string url, NameValueCollection headers)
        {

            HttpCommand webCommand = new HttpCommand(url, "Get");
            webCommand.Headers = headers;
            return webCommand.Execute();
        }
        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <param name="strData">数据</param>
        /// <returns></returns>
        public static string Post(string strUrl, string strData)
        {
            HttpCommand Webcommand = new HttpCommand(strUrl, "Post");
            if (!string.IsNullOrEmpty(strData))
            {
                Webcommand.ContentType = "application/x-www-form-urlencoded";
                Webcommand.Data = strData;
            }
            return Webcommand.Execute();
        }

        public static string Post(string url, string data, NameValueCollection headers)
        {
            HttpCommand webCommand = new HttpCommand(url, "Post");
            webCommand.Headers = headers;
            if (!string.IsNullOrEmpty(data))
            {
                webCommand.ContentType = "application/json";
                webCommand.Data = data;
            }
            return webCommand.Execute();
        }
        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <param name="strData">数据</param>
        /// <returns></returns>
        public static string Put(string strUrl, string strData)
        {
            HttpCommand Webcommand = new HttpCommand(strUrl, "Put");
            if (!string.IsNullOrEmpty(strData))
            {
                Webcommand.ContentType = "application/x-www-form-urlencoded";
                Webcommand.Data = strData;
            }
            return Webcommand.Execute();
        }
        /// <summary>
        /// Delete方法
        /// </summary>
        /// <param name="strUrl">链接地址</param>
        /// <returns></returns>
        public static string Delete(string strUrl)
        {
            HttpCommand Webcommand = new HttpCommand(strUrl, "DELETE");
            return Webcommand.Execute();
        }
    }
}
