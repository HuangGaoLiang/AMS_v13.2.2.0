using AMS.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：添加或修改的数据
    /// <para>作   者：瞿琦</para>
    /// <para>创建-间：2018-9-15</para>
    /// </summary>
    public class CouponRuleAuditRequest
    {
        /// <summary>
        /// 下一步审核人
        /// </summary>
        public string AuditUserId { get; set; }
        /// <summary>
        /// 下一步审核人姓名
        /// </summary>
        public string AuditUserName { get; set; }
        /// <summary>
        /// 奖学金详细信息
        /// </summary>
        public CouponRuleDetailRequest CouponRuleAuditDetail { get; set; }
    }

    /// <summary>
    /// 描述：奖学金详细信息
    /// <para>作   者：瞿琦</para>
    /// <para>创建-间：2018-9-15</para>
    /// </summary>
    public class CouponRuleDetailRequest
    {
        ///// <summary>
        ///// 所属校区
        ///// </summary>
        //public string SchoolId { get; set; }

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
    }
}
