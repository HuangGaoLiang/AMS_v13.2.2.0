/****************************************************************************\
所属系统:运营中心
所属模块:基础数据模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using JDW.SDK.Response;
using AMS.API.Controllers;
using AMS.Anticorrosion.JDW;

namespace OMC.API.Controllers
{
    /// <summary>
    ///  描    述：公司信息控制器
    ///  <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-23</para>
    /// </summary>
    [Route("api/AMS/Company")]
    [ApiController]
    public class CompanyController : BaseController
    {
        /// <summary>
        /// 获取所有公司
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-23</para>
        /// </summary>
        /// <returns>公司信息列表</returns>
        [HttpGet]
        public List<CompanyListResponse> Get()
        {
            var result = new JDWService().GetCompanyList();
            return result;
        }
    }
}
