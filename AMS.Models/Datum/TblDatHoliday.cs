using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 停课日表
    /// </summary>
    public partial class TblDatHoliday
    {
        /// <summary>
        /// 主键 停课日表
        /// </summary>
        public long HolidayId { get; set; }
        /// <summary>
        /// 所属校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// BeginDate
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// EndDate
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
