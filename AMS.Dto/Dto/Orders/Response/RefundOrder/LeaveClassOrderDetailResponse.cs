using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退费订单详情
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 退费单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 退费日期
        /// </summary>
        public DateTime RefundDate { get; set; }
        /// <summary>
        /// 提交人Id
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 提交人姓名
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 报名课程
        /// </summary>
        public List<RefundOrderTransactDetailListResponse> EnrollCourseList { get; set; }
        /// <summary>
        /// 停课日期
        /// </summary>
        public DateTime StopClassDate { get; set; }
        /// <summary>
        /// 票据情况
        /// </summary>
        public int ReceiptStatus { get; set; }
        /// <summary>
        /// 票据情况名
        /// </summary>
        public string ReceiptName { get; set; }
        /// <summary>
        /// 扣款合计
        /// </summary>
        public List<CostDetail> TotalDeductAmount { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退款方式
        /// </summary>
        public RefundType RefundType { get; set; }
        /// <summary>
        /// 退费原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AttchmentDetailResponse> AttachmentUrl { get; set; }
        /// <summary>
        /// 发卡银行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string BankUserName { get; set; }

    }

    /// <summary>
    /// 描述：扣除费用信息
    /// 作    者：瞿琦
    /// 创建时间：2018-11-8
    /// </summary>
    public class CostDetail
    {
        /// <summary>
        /// 费用名称
        /// </summary>
        public string CostName { get; set; }
        /// <summary>
        /// 扣除金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
