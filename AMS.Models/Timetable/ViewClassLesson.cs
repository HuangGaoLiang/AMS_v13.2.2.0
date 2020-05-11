using System;

namespace AMS.Models
{
    /// <summary>
    /// 家校互联－老师要上课的班级考勤列表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class ViewClassLesson
    {
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public string ClassRoom { get; set; }

        /// <summary>
        /// 课次类型 1：常规课  2：写生课
        /// </summary>
        public int LessonType { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 老师ID
        /// </summary>
        public string TeacherId { get; set; }
    }
}
