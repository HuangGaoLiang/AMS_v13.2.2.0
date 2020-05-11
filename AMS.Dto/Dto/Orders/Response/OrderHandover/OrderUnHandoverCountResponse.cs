using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单未交接汇总数据
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-15</para>
    /// </summary>
    public class OrderUnHandoverCountResponse
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员Id
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 招生专员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 未交接数量
        /// </summary>
        public int UnHandoverNumber { get; set; }
    }
}
