using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生扫码考勤
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public class ViewStudentScanCodeAttend
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int AttendStatus { get; set; }

        /// <summary>
        /// 调整状态
        /// </summary>
        public int AdjustType { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public DateTime? AttendDate { get; set; }

        /// <summary>
        /// 课次ID
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 1=TblTimLessonStudent表 2=TblTimReplenishLesson 表
        /// </summary>
        public int TblSource { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 上课老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 上课结束时间
        /// </summary>
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 校区ID
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int LessonType { get; set; }

        /// <summary>
        /// 业务ID 扫码暂时用到写生课
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }
    }
}
