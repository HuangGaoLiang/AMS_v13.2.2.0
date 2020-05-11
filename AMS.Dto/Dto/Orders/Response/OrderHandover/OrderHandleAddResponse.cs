using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：添加收款交接订单实体返回类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class OrderHandleAddResponse
    {
        /// <summary>
        /// 下一订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get; set; }
    }
}
