using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-休学
    /// </summary>
    public partial class TblOdrLeaveSchoolOrder
    {
        /// <summary>
        /// 主键(退费订单-休学)
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 休学日期
        /// </summary>
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }
        /// <summary>
        /// 退课总课次
        /// </summary>
        public int TotalRefundLessonCount { get; set; }
        /// <summary>
        /// 实际上课总课次
        /// </summary>
        public int TotalUseLessonCount { get; set; }
        /// <summary>
        /// 补充说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 休学原因
        /// </summary>
        public string Reason { get; set; }
    }
}
