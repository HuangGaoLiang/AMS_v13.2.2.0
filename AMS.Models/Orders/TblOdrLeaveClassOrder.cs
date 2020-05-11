using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-退班
    /// </summary>
    public partial class TblOdrLeaveClassOrder
    {
        /// <summary>
        /// 主键(退费订单-退班)
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 停课日期
        /// </summary>
        public DateTime StopClassDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 票据情况
        /// </summary>
        public int ReceiptStatus { get; set; }
    }
}
