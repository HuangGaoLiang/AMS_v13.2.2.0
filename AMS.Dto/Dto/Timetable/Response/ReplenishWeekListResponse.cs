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
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：补课周待补课汇总数据
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para> 
    /// </summary>
    public class ReplenishWeekListResponse
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课班级ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 缺课课次
        /// </summary>
        public int ClassNumber { get; set; }

        /// <summary>
        /// 补课周主键编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ReplenishWeekId { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 老师编号
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 补课时间
        /// </summary>
        public List<MakeUpTime> MakeUpTimeList { get; set; }
    }

    /// <summary>
    /// 补课时间
    /// </summary>
    public class MakeUpTime
    {
        /// <summary>
        /// 主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long AdjustLessonId { get; set; }

        /// <summary>
        /// 补课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }
    }
}
