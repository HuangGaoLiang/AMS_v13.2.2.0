using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描述：休学订单交易
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-10</para>
    /// </summary>
    internal class LeaveSchoolOrderTrade : RefundOrderTrade, IOrderTrade
    {
        private const string _remark="休学";     //备注信息
        /// <summary>
        /// 描述：休学订单交易对象实例化
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="refundOrder">休学订单信息</param>
        public LeaveSchoolOrderTrade(TblOdrRefundOrder refundOrder) : base(refundOrder)
        {
        }

        public OrderTradeType TradeType => OrderTradeType.LeaveSchoolOrder;    //订单交易类型

        /// <summary>
        /// 描述：获取休学订单交易信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <returns>订单交易信息</returns>
        /// <exception cref="AMS.Core.BussinessException">无</exception>
        public TradeInfo GetTradeInfo()
        {
            var entity = new TradeInfo
            {
                SchoolId = base.RefundOrder.SchoolId,
                OrderId = base.RefundOrder.RefundOrderId,
                OrderNo = base.RefundOrder.OrderNo,
                PayType = PayType.Other,
                TradeAmount = base.RefundOrder.Amount,  //无
                TradeBalanceAmount = base.RefundOrder.Amount,
                TotalDiscount = 0,
                Buyer = base.RefundOrder.StudentId.ToString(),
                Seller = base.RefundOrder.SchoolId,
                Remark= _remark
            };

            return entity;
        }

        /// <summary>
        /// 暂不实现
        /// </summary>
        /// <param name="tradeFailInfo"></param>
        public void TradeFail(TradeFailInfo tradeFailInfo)
        {
           // throw new NotImplementedException();
        }

        /// <summary>
        /// 暂不实现
        /// </summary>
        public void TradeSuccess()
        {
            //throw new NotImplementedException();
        }
    }
}
