using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 学生上课时间
    /// </summary>
    public class StudentTimetableDto
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime ClassBeginTime { get; set; }

        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime ClassEndTime { get; set; }
    }
}
