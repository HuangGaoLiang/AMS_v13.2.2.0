using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：课程简要信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-10-10 </para>
    /// </summary>
    public class CourseShortResponse
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程代码
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// 课程类别 0:必修课程 1:选修课程
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 课程中文名称
        /// </summary>
        public string CourseCnName { get; set; }

        /// <summary>
        /// 课程英文名称
        /// </summary>
        public string CourseEnName { get; set; }

        /// <summary>
        /// 班级中文名称
        /// </summary>
        public string ClassCnName { get; set; }

        /// <summary>
        /// 班级英文名称
        /// </summary>
        public string ClassEnName { get; set; }
    }
}
