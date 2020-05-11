using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 班级上课时间
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class AutClassTimeResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassTimeId { get; set; }
        /// <summary>
        /// 所属审核
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long AuditId { get; set; }
        /// <summary>
        /// 班级主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }
        /// <summary>
        /// 上课时间段主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// -1删除0不变1修改
        /// </summary>
        public DataStatus DataStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
