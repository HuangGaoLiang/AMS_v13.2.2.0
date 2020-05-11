using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 交接记录实体类
    /// </summary>
    public class ViewFinOrderHandover
    {
        /// <summary>
        /// 主健(订单交接核对主表)
        /// </summary>
        public long OrderHandoverId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员ID
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 招生专员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 交接金额（元）
        /// </summary>
        public decimal HandoverAmount { get; set; }

        /// <summary>
        /// 交接单数
        /// </summary>
        public int HandoverNumber { get; set; }

        /// <summary>
        /// 差异说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime HandoverDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }
    }
}
