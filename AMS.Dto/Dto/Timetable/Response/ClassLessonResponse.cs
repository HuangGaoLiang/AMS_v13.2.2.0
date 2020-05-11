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
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：家校互联－老师要上课的班级考勤列表。
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class ClassLessonResponse
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public string Week { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课时间段
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public string ClassRoom { get; set; }

        /// <summary>
        /// 课次类型 1：常规课  2：写生课
        /// </summary>
        public LessonType LessonType { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 标记 存在缺勤未补课
        /// </summary>
        public bool Mark { get; set; }
    }
}
