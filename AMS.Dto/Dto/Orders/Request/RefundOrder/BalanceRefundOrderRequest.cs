using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace AMS.Dto
{
    /// <summary>
    /// 描    述：余额退费请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-08</para>
    /// </summary>
    public class BalanceRefundOrderRequest : IRefundOrderTransactRequest
    {
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退费原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 发卡银行
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [JsonIgnore]
        public string CreatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [JsonIgnore]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 其他扣费项
        /// </summary>
        public List<CostRequest> OtherCostRequest { get; set; }

        /// <summary>
        /// 上传申请表附件
        /// </summary>
        public List<AttchmentAddRequest> AttachmentUrlList { get; set; }
    }
}
