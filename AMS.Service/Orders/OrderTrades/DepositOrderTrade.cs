using AMS.Dto;
using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描    述:  定金订单交易
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    internal class DepositOrderTrade : IOrderTrade
    {
        public OrderTradeType TradeType => OrderTradeType.DepositOrder;

        private readonly string _remark;
        private TblOdrDepositOrder _entity;

        /// <summary>
        /// 定金订单交易构造函数
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07</para>
        /// </summary>
        /// <param name="entity">订金实体信息</param>
        public DepositOrderTrade(TblOdrDepositOrder entity)
        {
            this._entity = entity;

            switch (entity.OrderStatus)
            {
                case (int)OrderStatus.Paid:
                    this._remark = "定金充值";
                    break;
                case (int)OrderStatus.Cancel:
                    this._remark = "定金作废";
                    break;
            }
        }

        /// <summary>
        /// 交易信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07</para>
        /// </summary>
        /// <returns>返回交易信息</returns>
        public TradeInfo GetTradeInfo()
        {
            TradeInfo info = new TradeInfo
            {
                SchoolId = _entity.SchoolId,
                OrderId = _entity.DepositOrderId,
                PayType = (PayType)_entity.PayType,
                TradeBalanceAmount = _entity.Amount,
                TradeAmount = 0,
                Buyer = _entity.StudentId.ToString(),
                OrderNo = _entity.OrderNo,
                Seller = _entity.SchoolId,
                Remark = _remark

            };
            return info;
        }

        /// <summary>
        /// 交易失败
        /// </summary>
        /// <param name="tradeFailInfo"></param>
        public void TradeFail(TradeFailInfo tradeFailInfo)
        {

        }

        /// <summary>
        /// 交易成功
        /// </summary>
        public void TradeSuccess()
        {

        }
    }
}
