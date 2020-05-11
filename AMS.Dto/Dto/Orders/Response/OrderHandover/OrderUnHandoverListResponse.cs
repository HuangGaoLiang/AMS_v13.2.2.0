using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单未交接列表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-15</para>
    /// </summary>
    public class OrderUnHandoverListResponse
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
        public int TradeType { get; set; }

        /// <summary>
        /// 收款类别
        /// </summary>
        public string TradeTypeName { get; set; }

        /// <summary>
        /// 状态(未交接/异常单/已核对/已交接)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态(未交接/异常单/已核对/已交接)
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
