using System;
using System.Collections.Generic;
using System.Text;
using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 余额订单交易
    /// </summary>
    internal class BalanceOrderTrade : RefundOrderTrade, IOrderTrade
    {
        /// <summary>
        /// 订单说明
        /// </summary>
        private readonly string _remark;

        /// <summary>
        /// 余额订单构造函数
        /// </summary>
        /// <param name="refundOrder">余额退费对象</param>
        /// <param name="remark">订单说明</param>
        public BalanceOrderTrade(TblOdrRefundOrder refundOrder, string remark) : base(refundOrder)
        {
            this._remark = remark;
        }

        public OrderTradeType TradeType => OrderTradeType.BalanceOrder;

        /// <summary>
        /// 获取交易信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <returns>交易信息</returns>
        public TradeInfo GetTradeInfo()
        {
            var entity = new TradeInfo
            {
                SchoolId = base.RefundOrder.SchoolId,
                OrderId = base.RefundOrder.RefundOrderId,
                OrderNo = base.RefundOrder.OrderNo,
                PayType = PayType.Other,
                TradeAmount = 0,
                TradeBalanceAmount = -base.RefundOrder.Amount,
                TotalDiscount = 0,
                Buyer = base.RefundOrder.StudentId.ToString(),
                Seller = base.RefundOrder.SchoolId,
                Remark = this._remark
            };
            return entity;
        }

        /// <summary>
        /// 交易失败
        /// </summary>
        /// <param name="tradeFailInfo"></param>
        public void TradeFail(TradeFailInfo tradeFailInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 交易成功
        /// </summary>
        public void TradeSuccess()
        {
            throw new NotImplementedException();
        }
    }
}
