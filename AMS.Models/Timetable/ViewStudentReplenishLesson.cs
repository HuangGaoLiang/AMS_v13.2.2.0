using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// K信-学生补课/调课安排列表
    /// </summary>
    public class ViewStudentReplenishLesson
    {
        /// <summary>
        /// 序号Id
        /// </summary>
        public long RowId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 教室ID
        /// </summary>
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 1=缺勤 2=请假 3=未开课
        /// </summary>
        public int Type { get; set; }
    }
}
