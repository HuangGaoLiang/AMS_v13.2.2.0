using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 退费订单-支付信息
    /// </summary>
    public partial class TblOdrRefundPay
    {
        /// <summary>
        /// 主键(退费订单-支付信息)
        /// </summary>
        public long RefundPayId { get; set; }
        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 退费订单ID
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 退款方式
        /// </summary>
        public int RefundType { get; set; }
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
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
