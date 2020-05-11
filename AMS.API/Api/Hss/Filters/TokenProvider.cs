using Jerrisoft.Platform.IdentityClient.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AMS.API.Api.Hss.Filters
{
    /// <summary>
    /// Token值提供者
    /// </summary>
    public class TokenProvider
    {
        /// <summary>
        /// 构造函数,静态方法访问的类,不让直接实例化
        /// </summary>
        protected TokenProvider()
        {

        }

        /// <summary>
        /// 取得token
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-27</para>
        /// </summary>
        /// <param name="context">HTTP请求上下文</param>
        /// <param name="tokenKey">token的KEY</param>
        /// <returns></returns>
        public static string GetToken(HttpContext context, string tokenKey)
        {
            string token = string.Empty;
            StringValues tokens = new StringValues();
            IEnumerable<string> result = new List<string>();
            bool rst = context.Request.Headers.TryGetValue(tokenKey, out tokens);
            if (rst)
            {
                token = tokens.FirstOrDefault();
            }
            return token;
        }
    }
}