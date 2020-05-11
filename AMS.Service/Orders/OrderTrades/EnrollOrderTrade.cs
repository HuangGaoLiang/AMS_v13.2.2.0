using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描    述:  报名订单交易
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    internal class EnrollOrderTrade : IOrderTrade
    {
        public OrderTradeType TradeType => OrderTradeType.EnrollOrder;
        private TblOdrEnrollOrder _entity;
        private readonly string _remark;

        /// <summary>
        /// 订单交易信息构造函数
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2018.9.19</para>
        /// </summary>
        /// <param name="entity">报名信息</param>
        public EnrollOrderTrade(TblOdrEnrollOrder entity)
        {
            this._entity = entity;
            switch (entity.OrderStatus)
            {
                case (int)OrderStatus.Paid:
                    this._remark = "报班支出";
                    break;
                case (int)OrderStatus.Cancel:
                    this._remark = "报班作废";
                    break;
            }
        }

        /// <summary>
        /// 交易信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018年11月6日 </para>
        /// </summary>
        /// <returns>返回订单信息</returns>
        public TradeInfo GetTradeInfo()
        {
            TradeInfo info = new TradeInfo
            {
                SchoolId = _entity.SchoolId,
                OrderId = _entity.EnrollOrderId,
                PayType = (PayType)_entity.PayType,
                TradeAmount = _entity.TotalTradeAmount * (-1),
                TradeBalanceAmount = _entity.UseBalance * (-1),
                TotalDiscount = _entity.TotalDiscountFee,
                Buyer = _entity.StudentId.ToString(),
                OrderNo = _entity.OrderNo,
                Seller = _entity.SchoolId,
                Remark = _remark
            };
            return info;
        }

        public void TradeFail(TradeFailInfo tradeFailInfo)
        {
            //throw new NotImplementedException();
        }

        public void TradeSuccess()
        {
            //throw new NotImplementedException();
        }


    }
}
