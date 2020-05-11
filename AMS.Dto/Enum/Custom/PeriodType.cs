/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
创建时间：2019-03-12
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
    /// 描    述：日期类型
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// 本周
        /// </summary>
        [Description("本周")]
        CurrentWeek = 1,
        /// <summary>
        /// 本月
        /// </summary>
        [Description("本月")]
        CurrentMonth = 2,
        /// <summary>
        /// 本学期
        /// </summary>
        [Description("本学期")]
        CurrentTerm = 3,
    }
}