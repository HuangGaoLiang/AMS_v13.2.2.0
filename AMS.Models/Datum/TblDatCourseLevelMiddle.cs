using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 课程级别设置中间表
    /// </summary>
    public partial class TblDatCourseLevelMiddle
    {
        /// <summary>
        /// 课程级别设置中间表主健(CourseLevelMiddleId)
        /// </summary>
        public long CourseLevelMiddleId { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 课程级别ID
        /// </summary>
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 开始年龄
        /// </summary>
        public int BeginAge { get; set; }
        /// <summary>
        /// 结束年龄
        /// </summary>
        public int EndAge { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
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
