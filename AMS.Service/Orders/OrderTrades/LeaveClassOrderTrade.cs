using System;
using System.Collections.Generic;
using System.Text;
using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退班订单交易
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-10</para>
    /// </summary>
    internal class LeaveClassOrderTrade : RefundOrderTrade, IOrderTrade
    {
        private readonly RefundType _refundType;   //退款方式
        private const string _remark = "退班转入";
        /// <summary>
        /// 描述：退班订单交易对象实例化
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-10</para>
        /// </summary>
        /// <param name="refundOrder">退费订单信息</param>
        /// <param name="refundType">退款方式</param>
        public LeaveClassOrderTrade(TblOdrRefundOrder refundOrder, RefundType refundType) : base(refundOrder)
        {
            _refundType = refundType;
        }

        public OrderTradeType TradeType => OrderTradeType.LeaveClassOrder;   //订单交易类型

        /// <summary>
        /// 描述：获取报班退费订单交易信息
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
                TradeAmount = base.RefundOrder.Amount,   //无
                TradeBalanceAmount = _refundType == RefundType.RefundToBalance ? base.RefundOrder.Amount : 0,
                TotalDiscount = 0,
                Buyer = base.RefundOrder.StudentId.ToString(),
                Seller = base.RefundOrder.SchoolId,
                Remark= _refundType== RefundType.RefundToBalance? _remark : ""
            };
            return entity;
        }

        /// <summary>
        /// 此方法没有此种场景，咱不实现
        /// </summary>
        /// <param name="tradeFailInfo"></param>
        public void TradeFail(TradeFailInfo tradeFailInfo)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 此方法没有此种场景，咱不实现
        /// </summary>
        public void TradeSuccess()
        {
            //throw new NotImplementedException();
        }
    }
}
