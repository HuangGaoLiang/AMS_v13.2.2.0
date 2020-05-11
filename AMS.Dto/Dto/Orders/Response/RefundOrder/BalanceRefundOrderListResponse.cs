using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：余额退费列表信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-17</para>
    /// </summary>
    public class BalanceRefundOrderListResponse : IOrderListResponse
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

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
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string GuardianMobile { get; set; }

        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 扣款金额
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
        /// 退费日期
        /// </summary>
        public DateTime RefundDate { get; set; }
    }
}
