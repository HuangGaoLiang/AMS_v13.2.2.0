using AMS.Dto.Enum;
using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：优惠券列表筛选条件
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class CouponRuleListSearchRequest : Page
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [JsonIgnore]
        public string SchoolNo { get; set; }
        /// <summary>
        /// 奖学金赠与类型
        /// </summary>
        public CouponType CouponType { get; set; }
        /// <summary>
        /// 奖学金状态
        /// </summary>
        public CouponRuleStatus EffectType { get; set; }
        /// <summary>
        /// 优惠名称
        /// </summary>
        public string CouponRuleName { get; set; }
    }
}
