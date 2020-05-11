using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 订单交接核对明细汇总
    /// </summary>
    public class ViewFinOrderHandoverDetailSummary
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 收银员Id
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 未交接总金额（元）
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 未交接单数
        /// </summary>
        public int UnHandoverNumber { get; set; }
    }
}
