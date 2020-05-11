/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
创建时间：2019-03-07
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课程状态
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public enum LessonStatus
    {
        /// <summary>
        /// 空注释
        /// </summary>
        [Description("")]
        None = -1,
        /// <summary>
        /// 未考勤
        /// </summary>
        [Description("未考勤")]
        NotCheckIn = 0,
        /// <summary>
        /// 已考勤
        /// </summary>
        [Description("已考勤")]
        Attended = 1,
        /// <summary>
        /// 请假
        /// </summary>
        [Description("请假")]
        Leave = 2,
        /// <summary>
        /// 缺勤
        /// </summary>
        [Description("缺勤")]
        Absent = 8,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 9,
        /// <summary>
        /// 已安排补课
        /// </summary>
        [Description("已安排补课")]
        Fixed = 11,
        /// <summary>
        /// 已安排调课
        /// </summary>
        [Description("已安排调课")]
        Adjusted = 12,
        /// <summary>
        /// 已安排调课
        /// </summary>
        [Description("补课周补课")]
        FixedWeek = 13,
        /// <summary>
        /// 已安排调课
        /// </summary>
        [Description("补签未确认")]
        SupplementNotConfirmed = 14,
        /// <summary>
        /// 已安排调课
        /// </summary>
        [Description("补签已确认")]
        SupplementConfirmed = 15
    }
}