/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// TblCashOrderTrade
    /// </summary>
    public partial class TblCashOrderTrade
    {
        /// <summary>
        /// 主键（TblCashOrderTrade）
        /// </summary>
        public long OrderTradeId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 交易类型（1 报名订单 2退班退费订单 3游学营退费订单 4余额退费订单 5订金定单 6休学订单）
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 付款方式(1刷卡 2现金 3转账 4微信 5支付宝 9其他)
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 使用余额支付
        /// </summary>
        public decimal TradeBalanceAmount { get; set; }

        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal TotalDiscount { get; set; }

        /// <summary>
        /// 买家(FROM)
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// 卖家(TO)
        /// </summary>
        public string Seller { get; set; }

        /// <summary>
        /// 交易状态（-1交易取消 1交易中 2交易完成）
        /// </summary>
        public int TradeStatus { get; set; }
        /// <summary>
        /// 订单交易说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
