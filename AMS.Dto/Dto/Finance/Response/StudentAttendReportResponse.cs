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
    /// 描    述：学生考勤统计表实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendReportResponse
    {
        /// <summary>
        /// 考勤日期信息
        /// </summary>
        public List<StudentAttendDateInfoResponse> DateInfos { get; set; }

        /// <summary>
        /// 学生考勤信息
        /// </summary>
        public List<StudentAttendInfoResponse> StudentAttendList { get; set; }

        /// <summary>
        /// 教师考勤信息
        /// </summary>
        public List<StudentAttendTeacherInfoResponse> TeacherAttendList { get; set; }
    }
}
