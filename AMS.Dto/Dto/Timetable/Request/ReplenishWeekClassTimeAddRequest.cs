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
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 补课周进行补课的数据
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public class ReplenishWeekClassTimeAddRequest
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [Required]
        public long CourseId { get; set; }

        /// <summary>
        /// 学期编号
        /// </summary>
        [Required]
        public long TermId { get; set; }

        /// <summary>
        /// 上课班级
        /// </summary>
        [Required]
        public long ClassId { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        [Required]
        public string TeacherId { get; set; }

        /// <summary>
        /// 补课信息
        /// </summary>
        public List<WeekClassTime> WeekClassTimeList { get; set; }

        /// <summary>
        /// 所属报名项
        /// </summary>
        [JsonIgnore]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 课程级别ID
        /// </summary>
        [JsonIgnore]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 新班级编号
        /// </summary>
        [JsonIgnore]
        public long NewClassId { get; set; }
    }

    /// <summary>
    /// 补课周信息
    /// </summary>
    public class WeekClassTime
    {
        /// <summary>
        /// 上课日期
        /// </summary>
        [Required]
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        [Required]
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        [Required]
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 上课教室
        /// </summary>
        [Required]
        public long ClassRoomId { get; set; }
    }
}
