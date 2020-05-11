using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 上课时间段表
    /// </summary>
    public partial class TblDatSchoolTime
    {
        /// <summary>
        /// 上课时间表主键
        /// </summary>
        public long SchoolTimeId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学期主健
        /// </summary>
        public long TermId { get; set; }
        /// <summary>
        /// 星期几
        /// </summary>
        public int WeekDay { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 时间段编号
        /// </summary>
        public string ClassTimeNo { get; set; }
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
