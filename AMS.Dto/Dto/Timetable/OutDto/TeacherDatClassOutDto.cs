using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：老师班级
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class TeacherDatClassOutDto
    {
        /// <summary>
        /// 学期编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }
    }
}
