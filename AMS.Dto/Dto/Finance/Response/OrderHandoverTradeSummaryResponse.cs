using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 财务交接订单列表信息(按订单类型汇总)
    /// </summary>
    public class OrderHandoverTradeSummaryResponse
    {
        /// <summary>
        /// 收款类别
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 收款类别名称
        /// </summary>
        public string OrderTradeTypeName { get; set; }

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
        /// 合计
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
