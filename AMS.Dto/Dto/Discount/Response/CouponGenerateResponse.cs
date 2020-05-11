using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：优惠数据生成产出
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponGenerateResponse
    {

        /// <summary>
        /// 优惠规则Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CouponRuleId { get; set; }
        /// <summary>
        /// 优惠券Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CouponId { get; set; }

        /// <summary>
        /// 赠与奖学金优惠金额
        /// </summary>
        public decimal CouponRuleAmount { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType CouponType { get; set; }
    }
}
