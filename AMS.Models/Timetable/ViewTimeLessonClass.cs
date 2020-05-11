using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学期班级学生实体
    /// </summary>
    public class ViewTimeLessonClass
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 学期Id
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 报名项Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }
    }
}
