using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单已交接列表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderHandoverListResponse
    {
        /// <summary>
        /// 订单交接Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderHandoverId { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime HandoverDate { get; set; }

        /// <summary>
        /// 招生专员
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 驻校出纳
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 交接金额（元）
        /// </summary>
        public decimal HandoverAmount { get; set; }

        /// <summary>
        /// 交接单数
        /// </summary>
        public int HandoverNumber { get; set; }
    }
}
