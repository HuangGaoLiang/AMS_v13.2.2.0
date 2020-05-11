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
using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：老师班级课表信息
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-7</para>
    /// </summary>
    public class TeacherClassTimetableResponse
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }
        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassDate { get; set; }
       
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string ClassTime { get; set; }
        /// <summary>
        /// 星期几
        /// </summary>
        public string Week { get; set; }
        ///// <summary>
        ///// 开始上课时间
        ///// </summary>
        //public string ClassBeginTime { get; set; }
        ///// <summary>
        ///// 上课结束时间
        ///// </summary>
        //public string ClassEndTime { get; set; }

    }
}
