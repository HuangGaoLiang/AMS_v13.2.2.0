/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：学生的考勤课次信息，包括缺勤、请假、未上课
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-27</para>
    /// </summary>
    public class StudentAttendanceLessonResponse
    {
        /// <summary>
        /// 课次编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassTime { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public string Week { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        [JsonIgnore]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        [JsonIgnore]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 时间过去标志（0时间未过去；1时间已过去）
        /// K信 1=缺勤 2=请假 3=未开课
        /// </summary>
        public int TimePastFlag { get; set; }

        /// <summary>
        /// 考勤状态（0未考勤；1已考勤；2请假）
        /// </summary>
        public int AttendStatus { get; set; }


    }
}
