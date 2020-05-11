using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 撤销排课视图类
    /// </summary>
    public class ViewCancelMakeLesson
    {
        /// <summary>
        /// 主健(课次基础信息表)课次ID
        /// </summary>
        public long LessonId { get; set; }

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
        /// 上课班级ID
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 上课老师ID
        /// </summary>
        public string TeacherId { get; set; }

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
        /// 考勤状态(0正常签到 1补签 2请假)
        /// </summary>
        public int AttendStatus { get; set; }
    }
}
