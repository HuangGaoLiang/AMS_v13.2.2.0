using AMS.Dto;
namespace AMS.Service
{
    /// <summary>
    /// 普能购买交易信息
    /// </summary>
    public class TradeInfo
    {
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PayType PayType { get; set; }

        /// <summary>
        /// 交易金额 (交易总额 订单交易总额=付款总额+使用余额总额)
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 使用余额支出或收入
        /// </summary>
        public decimal TradeBalanceAmount { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal TotalDiscount { get; set; }
        /// <summary>
        /// 买家(FROM),一般指学生
        /// </summary>
        public string Buyer { get; set; }
        /// <summary>
        /// 卖家(TO),一般指校区
        /// </summary>
        public string Seller { get; set; }
        /// <summary>
        /// 订单交易说明
        /// </summary>
        public string Remark { get; set; }
    }
}
