using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 一个课程数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseResponse
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

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 课程的级别数据
        /// </summary>
        public List<CourseLevelMiddleResponse> CourseLevels { get; set; }
    }

    /// <summary>
    /// 课程的级别数据
    /// </summary>
    public class CourseLevelMiddleResponse
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
