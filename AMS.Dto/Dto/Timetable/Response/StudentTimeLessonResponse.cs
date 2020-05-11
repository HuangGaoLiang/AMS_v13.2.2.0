using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生考勤实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    public class StudentTimeLessonResponse
    {
        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int TotalLessonCount { get; set; }

        /// <summary>
        /// 已上课次
        /// </summary>
        public int AttendedLessonCount { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int LeftLessonCount { get; set; }

        /// <summary>
        /// 常规课程
        /// </summary>
        public List<StudentTimeLessonListResponse> NormalLessons { get; set; }

        /// <summary>
        /// 写生课程
        /// </summary>
        public List<StudentTimeLessonListResponse> LifeClassLessons { get; set; }
    }
}
