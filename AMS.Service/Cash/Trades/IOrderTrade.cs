using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 订单交易
    /// </summary>
    public interface IOrderTrade
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        OrderTradeType TradeType { get; }
        /// <summary>
        /// 交易信息
        /// </summary>
        /// <returns></returns>
        TradeInfo GetTradeInfo();

        /// <summary>
        /// 交易失败
        /// </summary>
        void TradeFail(TradeFailInfo tradeFailInfo);
        /// <summary>
        /// 交易成功
        /// </summary>
        void TradeSuccess();
    }
}
