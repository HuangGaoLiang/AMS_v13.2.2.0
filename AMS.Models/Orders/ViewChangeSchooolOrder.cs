using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 转校订单
    /// </summary>
    public class ViewChangeSchooolOrder
    {
        /// <summary>
        /// 主键(TblOdrRefundOrder)
        /// </summary>
        public long RefundOrderId { get; set; }

        /// <summary>
        /// 所属校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型（1 报名订单 2退班退费订单 3游学营退费订单 4余额退费订单 5休学退费订单）
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 扣款合计
        /// </summary>
        public decimal TotalDeductAmount { get; set; }

        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单状态（-1订单取消0待付款1已付款/正常）
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 作废操作人
        /// </summary>
        public string CancelUserId { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelRemark { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
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
