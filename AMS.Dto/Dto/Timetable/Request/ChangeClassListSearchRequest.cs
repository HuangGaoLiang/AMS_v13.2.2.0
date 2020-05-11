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
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：转班明细列表查询条件
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ChangeClassListSearchRequest : Page
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期类型
        /// </summary>
        public long TermTypeId { get; set; }
        /// <summary>
        /// 转班日期开始时间
        /// </summary>
        public DateTime? ChangeClassTimeStart { get; set; }
        /// <summary>
        /// 转班日期结束时间
        /// </summary>
        public DateTime? ChangeClassTimeEnd { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
    }
}
