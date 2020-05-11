using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-转校转出
    /// </summary>
    public partial class TblOdrRefundChangeSchoolOrder
    {
        /// <summary>
        /// 主键(退费订单-转校转出)
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 转入校区
        /// </summary>
        public string InSchoolId { get; set; }
        /// <summary>
        /// 转出校区
        /// </summary>
        public string OutSchoolId { get; set; }
        /// <summary>
        /// 转校原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 转出日期
        /// </summary>
        public DateTime OutDate { get; set; }
        /// <summary>
        /// 票据情况 1未开发票2发票齐全3发票遗失
        /// </summary>
        public int ReceiptStatus { get; set; }
        /// <summary>
        /// 从余额转出多少金额
        /// </summary>
        public decimal TransFromBalance { get; set; }
        /// <summary>
        /// 退课总课次
        /// </summary>
        public int TotalRefundLessonCount { get; set; }
        /// <summary>
        /// 实际上课总课次
        /// </summary>
        public int TotalUseLessonCount { get; set; }
    }
}
