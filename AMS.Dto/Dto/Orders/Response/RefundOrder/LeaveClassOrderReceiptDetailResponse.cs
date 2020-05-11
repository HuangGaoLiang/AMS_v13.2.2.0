using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述:打印的收据详情
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderReceiptDetailResponse
    {
        /// <summary>
        /// 退款单位
        /// </summary>
        public string RefundUnit { get; set; }
        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime RefundDate { get; set; }
        /// <summary>
        /// 票据情况
        /// </summary>
        public int ReceiptStatus { get; set; }
        /// <summary>
        /// 停课日期
        /// </summary>
        public DateTime StopClassDate { get; set; }
        /// <summary>
        /// 扣款合计
        /// </summary>
        public List<CostDetail> TotalDeductAmount { get; set; }
        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 打印序号
        /// </summary>
        public string PrintNo { get; set; }
        /// <summary>
        /// 退班的课程
        /// </summary>
        public List<RefundOrderTransactDetailListResponse> RefundCourseList { get; set; }

    }
}
