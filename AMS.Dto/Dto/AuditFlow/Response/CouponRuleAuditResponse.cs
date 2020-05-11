using AMS.Core;
using AMS.Dto.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.Dto.AuditFlow
{
    /// <summary>
    /// 描述：赠与奖学金设置 审核信息
    /// <para>作   者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class CouponRuleAuditResponse
    {
        /// <summary>
        /// 审核人Id
        /// </summary>
        public string AuditUserId { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUserName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否是第一次提交数据的人
        /// </summary>
        public bool IsFirstSubmitUser { get; set; } = true;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 审核信息详情
        /// </summary>
        public List<CouponRuleAuditDetailResponse> Details { get; set; } = new List<CouponRuleAuditDetailResponse>();

    } 


    /// <summary>
    /// 赠与奖学金设置 审核的详情信息
    /// </summary>
    public class CouponRuleAuditDetailResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long AutCouponRuleId { get; set; }
        /// <summary>
        /// 优惠ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CouponRuleId { get; set; }

        /// <summary>
        /// 优惠名称
        /// </summary>
        public string CouponRuleName { get; set; }

        /// <summary>
        /// 优惠类型（1转介绍 2 满减奖学金 ）
        /// </summary>
        public CouponType CouponType { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

}
