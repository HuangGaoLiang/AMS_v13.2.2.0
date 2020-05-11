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
    /// 描述：班级上课时间调整提交的数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public class AdjustClassTimeRequest : IAdjustLessonRequest
    {
        ///// <summary>
        ///// 要更换课次的信息集合
        ///// </summary>
        //[Required(ErrorMessage = "请选择要更换老师的班级")]
        //public List<LessonInfoRequest> LessonInfoList { get; set; }
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime OldClassDate { get; set; }
        /// <summary>
        /// 新班级上课日期
        /// </summary>
        [Required(ErrorMessage = "请选择调整日期")]
        public DateTime NewClassDate { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        [Required(ErrorMessage = "请选择调整时间")]
        public string NewClassBeginTime { get; set; }
        /// <summary>
        /// 是否发送微信公众号通知
        /// </summary>
        public bool IsSend { get; set; } = false;
        ///// <summary>
        /// 通知内容
        /// </summary>
        public string Reason { get; set; }
    }
}
