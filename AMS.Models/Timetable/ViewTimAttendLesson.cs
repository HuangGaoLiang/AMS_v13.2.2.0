using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 描述：考勤的考勤信息
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-12</para>
    /// </summary>
    public class ViewTimAttendLesson
    {
        /// <summary>
        /// 课次Id
        /// </summary>
        public long LessonId { get; set; }
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 上课日期
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
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

    }
}
