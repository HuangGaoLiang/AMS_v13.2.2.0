using System;
using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 收款交接
    /// </summary>
    public class FinOrderHandoverResponse
    {
        /// <summary>
        /// 主健(订单交接核对主表)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderHandoverId { get; set; }

        /// <summary>
        /// 出纳
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 支付日期
        /// </summary>
        public DateTime? HandoverDate { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public List<string> Url { get; set; } = new List<string>();
    }
}
