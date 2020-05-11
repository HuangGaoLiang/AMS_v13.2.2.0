/****************************************************************************\
所属系统:招生系统
所属模块:课表模块
创建时间:
作   者:
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
    /// 描述：老师课表课次筛选条件
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-7</para>
    /// </summary>
    public class TeacherClassTimetableRequest
    {
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 上课日期 开始 （教师代课 时使用）
        /// </summary>
        public DateTime? BeginClassDate { get; set; }
        /// <summary>
        /// 上课日期  结束 (教师代课 时使用)
        /// </summary>
        public DateTime? EndClassDate { get; set; }
        /// <summary>
        ///  上课日期（全校上课日期/班级上课时间调整 时使用）
        /// </summary>
        public DateTime? ClassDate { get; set; }
    }
}
