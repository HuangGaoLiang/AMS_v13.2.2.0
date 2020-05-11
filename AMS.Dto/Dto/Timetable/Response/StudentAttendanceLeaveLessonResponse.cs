/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间：2019-03-21
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：家校互联-学生请假信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-21</para>
    /// </summary>
    public class StudentAttendanceLeaveLessonResponse
    {
        /// <summary>
        /// 课次编号
        /// </summary>
        public string LessonIds { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

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