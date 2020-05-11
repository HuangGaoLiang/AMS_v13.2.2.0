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

namespace AMS.Dto
{
    /// <summary>
    /// 描述：学生补课周的课次列表信息
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class ReplenishWeekClassTimeListResponse
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long AdjustLessonId { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 上课教室编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }
    }
}
