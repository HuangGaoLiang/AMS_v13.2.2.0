using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 业绩归属人
    /// </summary>
    public partial class TblOdrEnrollOrderAchieve
    {
        /// <summary>
        /// 主键(TblOdrEnrollOrderAchieve)
        /// </summary>
        public long EnrollOrderAchieveId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 所属订单
        /// </summary>
        public long EnrollOrderId { get; set; }
        /// <summary>
        /// 业绩归属人
        /// </summary>
        public string PersonalId { get; set; }
        /// <summary>
        /// 业绩比例
        /// </summary>
        public decimal Proportion { get; set; }
    }
}
