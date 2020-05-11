using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：奖学金金额生成
    /// <para>作  者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponAmountRequest
    {
        /// <summary>
        /// 是否全免
        /// </summary>
        public bool IsFreeAll { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [JsonIgnore]
        public string CreatorId { get; set; }


    }
}
