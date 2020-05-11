using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 课程列表数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseListResponse
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
        /// 课程级别
        /// </summary>
        public List<CourseListLevelResponse> CourseLevels { get; set; }
    }

    /// <summary>
    /// 课程列表的级别数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseListLevelResponse
    {
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
        public int SAge { get; set; }

        /// <summary>
        /// 结束年龄
        /// </summary>
        public int EAge { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
    }
}
