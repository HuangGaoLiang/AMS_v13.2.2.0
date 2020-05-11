using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次信息
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    public class StudentLessOutDto
    {
        /// <summary>
        /// 学生课次表主键编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonStudentId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonId { get; set; }

        /// <summary>
        /// 学生时间课次表 课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonStudentLessonId { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 补课调课类型
        /// </summary>
        public int AdjustType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 补课信息表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ReplenishLessonId { get; set; }

        /// <summary>
        /// 根课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RootLessonId { get; set; }

        /// <summary>
        /// 父课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ParentLessonId { get; set; }

        /// <summary>
        /// 补课信息表考勤状态
        /// </summary>
        public int RAttendStatus { get; set; }

        /// <summary>
        /// 所属报名项
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 上课班级ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 补课信息表补课调课类型
        /// </summary>
        public int RAdjustType { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程级别ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }
    }
}
