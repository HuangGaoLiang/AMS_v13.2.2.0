using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 班级课程表
    /// </summary>
    public partial class TblTimClassTime
    {
        /// <summary>
        /// 主健 班级上课时间课表
        /// </summary>
        public long ClassTimeId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 班级主健
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 上课时间段主健
        /// </summary>
        public long SchoolTimeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
