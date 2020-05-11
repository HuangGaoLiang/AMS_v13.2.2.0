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
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：补课,对缺勤或请假的进行补课
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class AdjustReplenishRequest : IAdjustLessonRequest
    {
        /// <summary>
        /// 学生ID
        /// </summary>
        [Required(ErrorMessage ="必填项")]
        public long StudentId { get; set; }

        /// <summary>
        /// 转出班级
        /// </summary>
        [Required(ErrorMessage = "必填项")]
        public long ClassId { get; set; }

        /// <summary>
        /// 转出日期
        /// </summary>
        [Required(ErrorMessage = "必填项")]
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 转入班级
        /// </summary>
        [Required(ErrorMessage = "必填项")]
        public long ToClassId { get; set; }

        /// <summary>
        /// 转入日期
        /// </summary>
        [Required(ErrorMessage = "必填项")]
        public DateTime ToClassDate { get; set; }
    }
}
