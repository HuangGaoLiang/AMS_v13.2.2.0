/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblCashOrderTrade仓储
    /// </summary>
    public class TblCashOrderTradeRepository : BaseRepository<TblCashOrderTrade>
    {
        public TblCashOrderTradeRepository()
        {
        }

        public TblCashOrderTradeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据订单Id和和订单号获取交易信息
        /// ---Caiyakang 2018-11-08
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="orderId">订单主健</param>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        public TblCashOrderTrade GetByOrderId(string schoolId, long orderId, string orderNo)
        {
            var entity = base.Load(x => x.SchoolId == schoolId
                                        && x.OrderId == orderId
                                        && x.OrderNo == orderNo);
            return entity;
        }
        /// <summary>
        /// 分页获取交易记录列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">交易记录请求对象</param>
        /// <param name="ignoreTradeTypeList">忽略的订单类型</param>
        /// <returns>交易记录列表</returns>
        public PageResult<TblCashOrderTrade> GetOrderTradeList(string schoolId, CashOrderTradeListRequest request, List<int> ignoreTradeTypeList = null)
        {
            var result = base.LoadQueryable()
                        .Where(x => x.SchoolId == schoolId && x.Buyer == request.StudentId.ToString() && x.TradeStatus == (int)TradeStatus.Complete)
                        .Where(x => !(ignoreTradeTypeList != null && ignoreTradeTypeList.Count > 0) || !ignoreTradeTypeList.Contains(x.TradeType));//忽略的订单类型
            return result.OrderByDescending(x => x.CreateTime).ToPagerSource(request.PageIndex, request.PageSize);
        }
    }
}
