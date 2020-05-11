using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 补课周信息表
    /// </summary>
    public partial class TblDatClassReplenishWeek
    {
        /// <summary>
        /// 主键 补课周信息表
        /// </summary>
        public long ReplenishWeekId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 老师编号
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
