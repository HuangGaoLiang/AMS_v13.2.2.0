using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 校区上课时间段
    /// </summary>
    public class DatSchoolTimResponse
    {
        /// <summary>
        /// 上课时间表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// 上课时间表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long NewSchoolTimeId { get; set; }

        /// <summary>
        /// A学期主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// B学期主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ToTermId { get; set; }

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
