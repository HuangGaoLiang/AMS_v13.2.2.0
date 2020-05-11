using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 描    述：报名学习计划
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public partial class TblOdrStudyPlan
    {
        /// <summary>
        /// 主健(报名学习计划)
        /// </summary>
        public long StudyPlanId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 课程级别ID
        /// </summary>
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 课程级别名称
        /// </summary>
        public string CourseLevelName { get; set; }
        /// <summary>
        /// 适用年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 时长(60为60分钟,90为90分钟)
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 显示优先级
        /// </summary>
        public int PriorityLevel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
