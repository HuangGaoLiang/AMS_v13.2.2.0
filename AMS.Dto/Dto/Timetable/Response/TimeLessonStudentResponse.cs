/****************************************************************************\
所属系统：XXX
所属模块：XXX
创建时间：
作    者：
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
    /// 描    述：学生课程信息实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class TimeLessonStudentResponse
    {
        /// <summary>
        /// 课次ID
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 考勤老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 班级老师Id
        /// </summary>
        public string ClassTeacherId { get; set; }

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
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 课次类型
        /// </summary>
        public int LessonType { get; set; }

        /// <summary>
        /// 考勤状态（0未考勤；1已考勤；2请假）
        /// </summary>
        public int AttendStatus { get; set; }
    }
}