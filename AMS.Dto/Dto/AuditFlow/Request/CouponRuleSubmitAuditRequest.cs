using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.Dto.AuditFlow
{
    /// <summary>
    /// 描述：赠与奖学金设置 审核信息
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class CouponRuleSubmitAuditRequest
    {
        /// <summary>
        /// 审核人Id
        /// </summary>
        public string AuditorId { get; set; }

        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string AuditorName { get; set; }

        /// <summary>
        /// 流程平台标题
        /// </summary>
        public string ApplyTitle { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }
    }

}
