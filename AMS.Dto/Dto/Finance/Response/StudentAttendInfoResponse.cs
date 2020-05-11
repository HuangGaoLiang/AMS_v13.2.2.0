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
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生考勤信息实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendInfoResponse
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long? StudentId { get; set; }

        /// <summary>
        /// 学生学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int TotalLessonCount { get; set; }

        /// <summary>
        /// 出勤总计
        /// </summary>
        public int AttendLessonCount { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int LeftLessonCount { get; set; }

        /// <summary>
        /// 当月出勤
        /// </summary>
        public string CurrentMonthLessonCount { get; set; }

        /// <summary>
        /// 考勤课程信息
        /// </summary>
        public List<StudentAttendLessonResponse> LessonDetails { get; set; }
    }
}
