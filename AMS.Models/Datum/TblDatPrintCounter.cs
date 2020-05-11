using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 打印调用记录
    /// </summary>
    public partial class TblDatPrintCounter
    {
        /// <summary>
        /// 主键  打印调用记录
        /// </summary>
        public long PrintCounterId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 打印类别
        /// </summary>
        public byte PrintBillType { get; set; }
        /// <summary>
        /// 打印次数
        /// </summary>
        public int Counts { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
