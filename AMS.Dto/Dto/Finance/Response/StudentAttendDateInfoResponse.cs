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

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课次考勤日期信息实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    public class StudentAttendDateInfoResponse
    {
        /// <summary>
        /// 课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 课程日期
        /// </summary>
        public string LessonDate { get; set; }

        /// <summary>
        /// 课程类型：1常规课2写生课
        /// </summary>
        public int LessonType { get; set; }
    }
}
