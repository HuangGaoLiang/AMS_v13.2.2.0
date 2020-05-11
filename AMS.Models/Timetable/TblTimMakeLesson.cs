using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 排课表
    /// </summary>
    public partial class TblTimMakeLesson
    {
        /// <summary>
        /// 主健(排课表)
        /// </summary>
        public long MakeLessonId { get; set; }
        /// <summary>
        /// 报名订单明细ID
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 上课班级ID
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 排课课次
        /// </summary>
        public int ClassTimes { get; set; }
        /// <summary>
        /// 是否确认排课
        /// </summary>
        public bool IsConfirm { get; set; }
        /// <summary>
        /// 首次上课时间
        /// </summary>
        public DateTime FirstClassTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
