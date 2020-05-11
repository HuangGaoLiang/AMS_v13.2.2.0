using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 财务交接订单列表信息(按日期汇总)
    /// </summary>
    public class OrderHandoverDaySummaryResponse
    {
        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 交接订单数
        /// </summary>
        public int HandoverNumber { get; set; }

        /// <summary>
        /// 现钞
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// 刷卡/微信/支付宝
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 转账
        /// </summary>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public decimal OtherAmount { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal BalanceAmount { get; set; }

        /// <summary>
        /// 使用奖学金
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
