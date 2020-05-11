using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-订单课程明细
    /// </summary>
    public partial class TblOdrRefundOrdeEnroll
    {
        /// <summary>
        /// 主键(退费订单-订单课程明细)
        /// </summary>
        public long RefundOrderEnrollId { get; set; }
        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 报名订单课程明细ID
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 退费订单ID
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 退课课次数量
        /// </summary>
        public int LessonCount { get; set; }
        /// <summary>
        /// 实际上课课次
        /// </summary>
        public int UseLessonCount { get; set; }
        /// <summary>
        /// 已上课课次单价
        /// </summary>
        public decimal LessonPrice { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
