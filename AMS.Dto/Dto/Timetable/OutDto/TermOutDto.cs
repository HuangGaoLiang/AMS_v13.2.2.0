using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：学期信息
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class TermOutDto
    {
        /// <summary>
        /// 学期编号
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
