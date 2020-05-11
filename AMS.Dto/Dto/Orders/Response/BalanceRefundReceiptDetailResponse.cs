using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：余额退费打印实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-17</para>
    /// </summary>
    public class BalanceRefundReceiptDetailResponse
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 退费原因
        /// </summary>
        public string Remark { get; set; }

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
        /// 经办人
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 打印序号
        /// </summary>
        public string PrintNo { get; set; }

        /// <summary>
        /// 收款单位
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 扣款详情
        /// </summary>
        public List<CostDetail> CostList { get; set; }
    }
}
