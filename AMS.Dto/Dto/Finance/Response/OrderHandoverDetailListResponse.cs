using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单交接明细信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderHandoverDetailListResponse
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生id
        /// </summary>
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
        /// 收款金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 收款类别
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 收款类别
        /// </summary>
        public string OrderTradeTypeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime? HandoverDate { get; set; }

        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime? PayDate { get; set; }
    }
}
