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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 描述：结果集操作类
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class ResultDataUtil
    {
        /// <summary>
        /// 不允许直接实例化
        /// </summary>
        protected ResultDataUtil()
        {

        }

        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <typeparam name="T">数据库</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ContentResult GetResult<T>(T data)
        {
            ContentResult result = new ContentResult();
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            result.Content = JsonConvert.SerializeObject(data, jsonSettings);
            return result;
        }
    }
}
