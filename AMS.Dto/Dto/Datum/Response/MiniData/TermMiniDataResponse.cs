using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 下拉框数据--学期
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TermMiniDataResponse
    {
        /// <summary>
        /// 学期Id主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }
        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }
        
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
    }
}
