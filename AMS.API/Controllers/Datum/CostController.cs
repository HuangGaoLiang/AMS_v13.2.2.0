using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AMS.Service;
using Jerrisoft.Platform.Exception;
using AMS.Core;
using AMS.API.Controllers;
using AMS.Dto;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 学期资源 REST
    /// </summary>
    [Route("api/AMS/Cost")]
    [ApiController]
    public class CostController : BaseController
    {
        /// <summary>
        /// 转校费用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-28</para>
        /// </summary>
        /// <returns>转校费用列表</returns>
        [HttpGet, Route("GetChangeSchoolFee")]
        public List<CostResponse> GetChangeSchoolFee()
        {
            return CostService.GetCosts(Dto.TypeCode.CHANGE_SCHOOL_FEE);
        }

        /// <summary>
        /// 描述：退班退费费用
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>退班费用列表</returns>
        [HttpGet, Route("GetLeaveClassFee")]
        public List<CostResponse> GetLeaveClassFee()
        {
            return CostService.GetCosts(Dto.TypeCode.LEAVE_CLASS_FEE);
        }

        /// <summary>
        /// 余额退费费用
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetBalanceFee")]
        public List<CostResponse> GetBalanceFee()
        {
            return CostService.GetCosts(Dto.TypeCode.BALANCE_FEE);
        }
    }
}
