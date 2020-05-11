using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    ///  描述：奖学金返回实体类
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponResponse
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
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否全免
        /// </summary>
        public bool IsFreeAll { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string CouponType { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonIgnore]
        public long StudentId { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
    }
}
