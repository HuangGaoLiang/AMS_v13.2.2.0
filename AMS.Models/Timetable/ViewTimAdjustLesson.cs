using System;

namespace AMS.Models
{
    /// <summary>
    /// 补课周补课时间
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class ViewTimAdjustLesson
    {

        /// <summary>
        /// 主键编号
        /// </summary>
        public long AdjustLessonId { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 教室编号
        /// </summary>
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 课次编号
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }
    }
}
