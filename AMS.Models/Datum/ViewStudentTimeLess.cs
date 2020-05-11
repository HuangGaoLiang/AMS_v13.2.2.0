using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生缺勤数据
    /// </summary>
    public class ViewStudentTimeLess
    {
        /// <summary>
        /// 课次编号
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 学生主键编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 上课班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 老师编号
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 学期编号
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 课程等级编号
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

    }
}
