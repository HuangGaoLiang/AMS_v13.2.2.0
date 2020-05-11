using AMS.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 上课时间
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class MakeLessonResponse
    {
        /// <summary>
        /// 班级主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string CourseLeaveName { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public List<string> Week { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public List<string> ClassTime { get; set; }
    }
}
