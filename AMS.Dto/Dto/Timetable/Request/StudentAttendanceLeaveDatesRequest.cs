/****************************************************************************\
所属系统：招生系统
所属模块：家校互联
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
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课程请假时间列表请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-07</para>
    /// </summary>
    public class StudentAttendanceLeaveDatesRequest
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
        /// 月数
        /// </summary>
        public int MonthsCount { get; set; }
    }
}