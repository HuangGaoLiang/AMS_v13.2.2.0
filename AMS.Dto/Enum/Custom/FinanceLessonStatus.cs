/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-15
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：考勤统计表课程状态
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public enum FinanceLessonStatus
    {
        /// <summary>
        /// 转校
        /// </summary>
        [Description("转校")]
        ChangeSchool = 1,
        /// <summary>
        /// 转班
        /// </summary>
        [Description("转班")]
        ChangeClass = 2,
        /// <summary>
        /// 休学
        /// </summary>
        [Description("休学")]
        LeaveSchool = 3,
        /// <summary>
        /// 退费
        /// </summary>
        [Description("退费")]
        Refund = 4
    }
}