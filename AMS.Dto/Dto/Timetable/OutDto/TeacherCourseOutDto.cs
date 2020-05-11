using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：老师课程
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class TeacherCourseOutDto
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程中文名称
        /// </summary>
        public string ClassCnName { get; set; }

        /// <summary>
        /// 教室主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }
    }
}
