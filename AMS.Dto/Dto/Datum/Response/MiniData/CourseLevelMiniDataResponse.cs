using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 下拉框数据--课程
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseLevelMiniDataResponse
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 课程等级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string CourseLevelName { get; set; }

        /// <summary>
        /// 开始年龄
        /// </summary>
        public string SAge { get; set; }

        /// <summary>
        /// 结束年龄
        /// </summary>
        public string EAge { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public string Duration { get; set; }
    }
}
