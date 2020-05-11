using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：课程信息包含等级
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-11-07 </para>
    /// </summary>
    public class CourseAndLevelResponse
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }


        /// <summary>
        /// 课程类别 0:必修课程 1:选修课程
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 课程等级编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLeaveId { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string CourseLeaveName { get; set; }

    }
}
