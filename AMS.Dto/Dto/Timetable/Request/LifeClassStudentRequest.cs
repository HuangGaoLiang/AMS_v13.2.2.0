using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课学生列表请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class LifeClassStudentRequest
    {
        /// <summary>
        /// 学期Id
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long? CourseId { get; set; }

        /// <summary>
        /// 课程级别Id
        /// </summary>
        public long? CourseLevelId { get; set; }
    }
}
