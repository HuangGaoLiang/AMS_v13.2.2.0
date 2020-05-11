using AMS.Dto;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using AMS.Service;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：缴费交易控制器
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/CashOrderTrade")]
    [ApiController]
    public class CashOrderTradeController : BsnoController
    {
        /// <summary>
        /// 获取交易记录列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="request">交易记录查询条件</param>
        /// <returns>缴费交易记录分页列表</returns>
        [HttpGet, Route("GetOrderTradeList")]
        public PageResult<CashOrderTradeListResponse> GetOrderTradeList([FromQuery]CashOrderTradeListRequest request)
        {
            return TradeService.GetOrderTradeList(base.SchoolId, request);
        }
    }
}
