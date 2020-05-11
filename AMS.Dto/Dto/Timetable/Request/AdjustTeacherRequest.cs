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
    /// 描述：教师代课调整的提交数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：201903-8</para>
    /// </summary>
    public class AdjustTeacherRequest : IAdjustLessonRequest
    {
        /// <summary>
        /// 要更换课次的信息集合
        /// </summary>
        [Required(ErrorMessage = "请选择要更换老师的班级")]
        public List<LessonInfoRequest> LessonInfoList { get; set; }
        /// <summary>
        /// 老师Id
        /// </summary>
        [Required(ErrorMessage = "请选择代课老师")]
        public string TeacherId { get; set; }
        /// <summary>
        /// 是否发送微信公众号通知
        /// </summary>
        public bool IsSend { get; set; } = false;
        /// <summary>
        /// 理由
        /// </summary>
        public string Reason { get; set; }
    }

    /// <summary>
    /// 要更换信息的课次信息
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-10</para>
    /// </summary>
    public class LessonInfoRequest
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }
    }
}
