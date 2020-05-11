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

using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次考勤老师信息列表实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendTeacherInfoResponse
    {
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 出勤总计
        /// </summary>
        public decimal AttendLessonCount { get; set; }

        /// <summary>
        /// 当月出勤
        /// </summary>
        public string CurrentMonthLessonCount { get; set; }

        /// <summary>
        /// 签名地址
        /// </summary>
        public string SignatureUrl { get; set; }

        /// <summary>
        /// 老师考勤列表
        /// </summary>
        public List<StudentAttendTeacherLessonResponse> TeacherLessons { get; set; }
    }
}
