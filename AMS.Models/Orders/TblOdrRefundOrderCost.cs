using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-其他费用
    /// </summary>
    public partial class TblOdrRefundOrderCost
    {
        /// <summary>
        /// 主键(退费订单-其他费用)
        /// </summary>
        public long RefundClassCostId { get; set; }
        /// <summary>
        /// 退费订单
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId{get;set; }

        /// <summary>
        /// 费用ID
        /// </summary>
        public long CostId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
