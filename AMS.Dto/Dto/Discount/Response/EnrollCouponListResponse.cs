using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：报名时显示要使用的奖学金券
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：20128-11-2</para>
    /// </summary>
    public class EnrollCouponListResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CouponId { get; set; }

        /// <summary>
        /// 券号
        /// </summary>
        public string CouponNo { get; set; }
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType CouponType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 是否全免
        /// </summary>
        public bool IsFreeAll { get; set; }
    }
}
