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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 补课周待补课汇总查询条件
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class ReplenishWeekListSearchRequest : Page
    {
        /// <summary>
        /// 学期编号
        /// </summary>
        [Required]
        public long TermId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 老师编号
        /// </summary>
        [JsonIgnore]
        public string TeacherId { get; set; }
    }
}
