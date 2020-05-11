using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TblDatCostType
    {
        /// <summary>
        /// 
        /// </summary>
        public long TblDatCostTypeId { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
