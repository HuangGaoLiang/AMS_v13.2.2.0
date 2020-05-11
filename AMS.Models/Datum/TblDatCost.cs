using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 费用类型表
    /// </summary>
    public partial class TblDatCost
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long CostId { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CostName { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
