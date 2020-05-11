using AMS.Core;
using AMS.Dto.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：赠与奖学金返回信息实体
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponRuleListResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CouponRuleId { get; set; }     

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 优惠名称
        /// </summary>
        public string CouponRuleName { get; set; }

        /// <summary>
        /// 优惠类型（1转介绍 2 满减奖学金 ）
        /// </summary>
        public byte CouponType { get; set; }

        /// <summary>
        /// 满多少
        /// </summary>
        public decimal FullAmount { get; set; }

        /// <summary>
        /// 优惠额度
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 名额限制
        /// </summary>
        public int MaxQuota { get; set; }

        /// <summary>
        /// 已使用人数
        /// </summary>
        public int UseQuota { get; set; }

        /// <summary>
        /// 优惠起始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 优惠截止日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 优惠原因
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否生效
        /// </summary>
        public CouponRuleStatus EffectType { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
