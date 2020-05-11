using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 费用名称
    /// </summary>
    public class CostResponse
    {
        /// <summary>
        /// 费用ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CostId { get; set; }
        /// <summary>
        /// 费用名称
        /// </summary>
        public string CostName { get; set; }
    }
}
