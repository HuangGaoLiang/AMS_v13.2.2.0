using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生课次表
    /// </summary>
    public class ViewTimLessonStudent
    {
        /// <summary>
        /// 主健(课次基础信息表)课次ID
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 所属报名项
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

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
        public int LessonType { get; set; }

        /// <summary>
        /// 占用课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 课次最终状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 主健(课次基础信息表)课次ID
        /// </summary>
        public long LessonStudentId { get; set; }

        /// <summary>
        /// 考勤状态(1正常 2请假)
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 1已经安排补课2已安排调课3已经安排补课周补课 4补签未确认5补签已确认
        /// </summary>
        public int AdjustType { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public DateTime? AttendDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
