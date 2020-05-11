/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-05
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Datum
{
    /// <summary>
    /// 描    述：家校互联配置控制器
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-05</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/HomeSchoolSetting")]
    [ApiController]
    public class HomeSchoolSettingController : BaseController
    {
        /// <summary>
        /// 描述：根据公司Id获取家校互联的配置列表信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-11</para>
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>家校互联的配置列表信息</returns>
        [HttpGet]
        public List<HomeSchoolSettingListResponse> Get(string companyId)
        {
            return  new HomeSchoolSettingService().GetSettingList(companyId);
        }

        /// <summary>
        /// 描述：更新家校互联配置信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-11</para>
        /// </summary>
        /// <param name="request">要更新的配置信息</param>
        [HttpPut]
        public void UpdateSetting(HomeSchoolSettingListRequest request)
        {
             new HomeSchoolSettingService().UpdateSetting(request);
        }
    }
}