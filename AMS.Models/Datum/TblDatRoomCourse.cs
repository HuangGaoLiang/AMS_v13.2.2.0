using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 教室课程表
    /// </summary>
    public partial class TblDatRoomCourse
    {
        /// <summary>
        /// 主健 教室课程表
        /// </summary>
        public long RoomCourseId { get; set; }
        /// <summary>
        /// 教室主健
        /// </summary>
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 开班时间段数
        /// </summary>
        public int MaxWeekStage { get; set; }
        /// <summary>
        /// 每个时间段学位数
        /// </summary>
        public int MaxStageStudents { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
