using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 完整的学生考勤(包括正常/补课/调课的考勤)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class ViewCompleteStudentAttendance
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
        /// TblTimReplenishLesson/TblTimLessonStudent 的主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 考勤状态(0正常签到 1补签 2请假)
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 该字段为冗余字段，TblTimReplenishLesson,1已经安排补课2已安排调课,
        /// </summary>
        public int AdjustType { get; set; }

        /// <summary>
        /// 考勤补签码
        /// </summary>
        public string ReplenishCode { get; set; }

        /// <summary>
        /// 签到人员类型 1老师9财务
        /// </summary>
        public int AttendUserType { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public DateTime? AttendDate { get; set; }

        /// <summary>
        /// 1=TblTimLessonStudent
        /// 2=TblTimReplenishLesson
        /// </summary>
        public int TblType { get; set; }

        /// <summary>
        /// 业务ID 比如写生课主键ID
        /// </summary>
        public long BusinessId { get; set; }
    }
}
