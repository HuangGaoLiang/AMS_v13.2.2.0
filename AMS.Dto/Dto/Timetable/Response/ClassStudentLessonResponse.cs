/****************************************************************************\
所属系统:招生系统
所属模块:课表模块-家校互联
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
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：家校互联－班级的学生考勤列表。
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class ClassStudentLessonResponse
    {
        /// <summary>
        /// 是否可以扫码考勤
        /// true=可以 false=未到时间
        /// </summary>
        public bool CanAttend { get; set; }

        /// <summary>
        /// 插班生
        /// </summary>
        public List<ClassStudent> TransferStudents { get; set; }

        /// <summary>
        /// 本班生
        /// </summary>
        public List<ClassStudent> Students { get; set; }
    }

    /// <summary>
    /// 上课学生
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class ClassStudent
    {
        /// <summary>
        /// 学生课次ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LessonId { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 0=未考勤
        /// 1=正常签到 显示考勤时间
        /// 2=补签未确认(缺勤) 显示撤回补签
        /// 3=补签已确认 显示考勤时间
        /// 4=请假(当前时间之前) 判断AdjustStatus是否已安排补课
        /// 5=缺勤 判断AdjustStatus是否已安排补课
        /// 6=请假(当前时间之后) 无需判断AdjustStatus是否已安排补课 显示 --
        /// 7=补签未确认(请假) 显示撤回补签
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 是否已安排调课了
        /// 是：补课、调课
        /// 否：显示
        /// </summary>
        public bool AdjustStatus { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public string AttendTime { get; set; } = string.Empty;
    }
}
