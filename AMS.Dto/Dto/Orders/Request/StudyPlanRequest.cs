using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 学习计划请求实体类
    /// </summary>
    public class StudyPlanRequest
    {
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程级别Id
        /// </summary>
        public long CourseLevelId { get; set; }
    }
}
