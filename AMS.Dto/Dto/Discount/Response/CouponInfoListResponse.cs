using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：获取奖学金券实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    public class CouponInfoListResponse
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
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int CouponType { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string CouponTypeName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime { get; set; }

        /// <summary>
        /// 状态(1未使用/2已使用/3已过期)
        /// </summary>
        public CouponStatus Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 是否全免
        /// </summary>
        public bool IsFreeAll { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
