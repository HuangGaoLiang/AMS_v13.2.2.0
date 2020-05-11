/****************************************************************************\
所属系统:招生系统
所属模块:公共枚举
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
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 业务日志枚举
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public enum LogBusinessType
    {
        /// <summary>
        /// 家校互联家长登陆
        /// </summary>
        [Description("家校互联家长登陆")]
        HssLogin=1,

        /// <summary>
        /// 学生档案信息修改
        /// </summary>
        [Description("学生档案信息修改")]
        Student = 2,

        /// <summary>
        /// 补课周删除
        /// </summary>
        [Description("补课周删除")]
        ReplenishWeekCancel = 3,
    }
}
