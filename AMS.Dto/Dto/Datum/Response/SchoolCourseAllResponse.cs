using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 一个校区已授权的课程详情,未授权的课程IsCheck为false
    /// </summary>
    public class SchoolCourseAllResponse
    {
        /// <summary>
        /// 选修课
        /// </summary>
        public List<SchoolCourseDetailResponse> RequiredCourse { get; set; }
        /// <summary>
        /// 必选
        /// </summary>
        public List<SchoolCourseDetailResponse> ElectiveCourse { get; set; }

    }

    /// <summary>
    /// 课程详情
    /// </summary>
    public class SchoolCourseDetailResponse
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 课程类别
        /// </summary>
        public int CourseType { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
