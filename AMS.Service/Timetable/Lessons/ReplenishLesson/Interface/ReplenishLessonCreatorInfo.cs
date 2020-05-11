using System;
using System.Collections.Generic;
using System.Text;
using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 调整课次创建实体类
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间</para>
    /// </summary>
    public class ReplenishLessonCreatorInfo
    {
        /// <summary>
        /// 转出课次Id
        /// </summary>
        public long OutLessonId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 所属报名项
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 学期ID
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 上课班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程级别ID
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 教室ID
        /// </summary>
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 课次类型
        /// </summary>
        public LessonType LessonType { get; set; } = LessonType.RegularCourse;

        /// <summary>
        /// 占用课次
        /// </summary>
        public int LessonCount { get; set; } = 1;

        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public LessonBusinessType BusinessType { get; set; }
    }
}
