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
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 补课周添加备注
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class ReplenishWeekModifyRemarkRequest
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        [JsonIgnore]
        public long ReplenishWeekId { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 上课老师编号
        /// </summary>
        [JsonIgnore]
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课学生编号
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 上课班级
        /// </summary>
        [Required]
        public long ClassId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get; set; }
    }
}
