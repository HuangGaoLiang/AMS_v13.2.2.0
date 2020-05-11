using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：余额退费详细信息（包含附件）
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-17</para>
    /// </summary>
    public class BalanceRefundOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 退费单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 扣款合计
        /// </summary>
        public decimal TotalDeductAmount { get; set; }

        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal RealRefundAmount { get; set; }

        /// <summary>
        /// 退费方式
        /// </summary>
        public string RefundTypeName { get; set; }

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
        /// 退费原因
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 退费日期
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 扣款详情
        /// </summary>
        public List<CostDetail> CostList { get; set; }

        /// <summary>
        /// 余额附件
        /// </summary>
        public List<AttchmentDetailResponse> AttachmentList { get; set; }
    }
}
