using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：课次调整信息
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-20</para>
    /// </summary>
    public class LessonAdjustOutDto
    {
        /// <summary>
        ///课次调整表Id
        /// </summary>
        public long AdjustLessonId { get; set; }
        /// <summary>
        /// 报名课程明细Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }
        /// <summary>
        /// 上课班级ID
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 教室ID
        /// </summary>
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 课程级别ID
        /// </summary>
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 占用课次
        /// </summary>
        public int LessonCount { get; set; }
        /// <summary>
        /// 课次类型
        /// </summary>
        public int LessonType { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 学期ID
        /// </summary>
        public long TermId { get; set; }
    }
}
