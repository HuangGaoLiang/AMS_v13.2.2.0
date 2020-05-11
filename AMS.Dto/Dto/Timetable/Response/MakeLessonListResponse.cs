using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 排课列表
     /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class MakeLessonListResponse
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 排课项
        /// </summary>
        public List<MakeLessonItemListResponse> MakeLessonItem { get; set; }
    }

    /// <summary>
    /// 排课项列表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class MakeLessonItemListResponse
    {
        /// <summary>
        /// 报名订单课程明细Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 已排课次
        /// </summary>
        public int ClassTimesUse { get; set; }

        /// <summary>
        /// 状态 0:未排课 1:待确认
        /// </summary>
        public int Status { get; set; }
    }
}
