/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次考勤课程实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendLessonResponse
    {
        /// <summary>
        /// 学生课次Id
        /// </summary>
        public string LessonStudentIds { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public LessonType LessonType { get; set; }

        /// <summary>
        /// 课程日期
        /// </summary>
        public string LessonDate { get; set; }

        /// <summary>
        /// 耗用课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int? AttendStatus { get; set; }

        /// <summary>
        /// 是否补课调课：0不是补课调课；1是补课调课
        /// </summary>
        public int? ReplenishStatus { get; set; }

        /// <summary>
        /// 补课调课信息
        /// </summary>
        public string ReplenishLessonTips { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }
    }
}
