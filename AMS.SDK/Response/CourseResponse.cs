using System.Collections.Generic;

namespace AMS.SDK
{
    /// <summary>
    /// 一个课程数据
    /// </summary>
    public class CourseResponse
    {
        /// <summary>
        /// 课程Id
        /// </summary>
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
        public List<CourseLevelResponse> CourseLevels { get; set; }
    }
}
