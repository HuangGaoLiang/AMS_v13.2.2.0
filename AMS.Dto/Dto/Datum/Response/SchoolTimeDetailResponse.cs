using System;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  上课时间段明细列表
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class SchoolTimeDetailResponse
    {
        /// <summary>
        /// 上课时间表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// 学期主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// 星期几
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 时间段编号
        /// </summary>
        public string ClassTimeNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
