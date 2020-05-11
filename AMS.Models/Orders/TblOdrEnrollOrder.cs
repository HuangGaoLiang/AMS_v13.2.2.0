using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 报名订单
    /// </summary>
    public partial class TblOdrEnrollOrder
    {
        /// <summary>
        /// 主键(报名订单)
        /// </summary>
        public long EnrollOrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 1新生2老生
        /// </summary>
        public int OrderNewType { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 报名校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 报名总课次
        /// </summary>
        public int TotalClassTimes { get; set; }
        /// <summary>
        /// 学费总额
        /// </summary>
        public decimal TotalTuitionFee { get; set; }
        /// <summary>
        /// 杂费总额
        /// </summary>
        public decimal TotalMaterialFee { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal TotalDiscountFee { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 交易总额(订单交易总额=付款总额+使用余额总额)
        /// </summary>
        public decimal TotalTradeAmount { get; set; } 
        /// <summary>
        /// 交易总额（实收金额）
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalance { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string CreateId { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 作废操作人
        /// </summary>
        public string CancelUserId { get; set; }

        /// <summary>
        /// 作废操作名称
        /// </summary>
        public string CancelUserName { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelDate { get; set; }
        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelRemark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
