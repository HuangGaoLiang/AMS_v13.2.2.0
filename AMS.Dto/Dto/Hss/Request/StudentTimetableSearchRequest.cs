/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:2019-03-08
作   者:郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto.Hss
{
    /// <summary>
    /// 描述：查询学生课表条件
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class StudentTimetableSearchRequest
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [Required]
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 1本周2本月3本学期
        /// </summary>
        public PeriodType PeriodType { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long? CourseId { get; set; }
    }
}
