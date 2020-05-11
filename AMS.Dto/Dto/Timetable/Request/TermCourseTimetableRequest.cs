using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 校区课程表提交审核
    /// </summary>
    public class TermTimetableRequest
    {
        /// <summary>
        /// 下一步审批人
        /// </summary>
        public string AuditUserId { get; set; }

        /// <summary>
        /// 下一步审批人名称
        /// </summary>
        public string AuditUserName { get; set; }

        /// <summary>
        /// 60分钟课程表
        /// </summary>
        public TermCourseTimetableRequest CourseTimetableSixty { get; set; }
        /// <summary>
        /// 90分钟课程表
        /// </summary>
        public TermCourseTimetableRequest CourseTimetableNinety { get; set; }
    }

    /// <summary>
    /// 课程表明细
    /// </summary>
    public class TermCourseTimetableRequest
    {
        /// <summary>
        /// 课程表的教室
        /// </summary>
        public List<TimetableClassRoomRequest> ClassRooms { get; set; }
    }

    /// <summary>
    /// 课程表的教室
    /// </summary>
    public class TimetableClassRoomRequest
    {
        /// <summary>
        /// 教室Id
        /// </summary>
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 课程表的班级信息
        /// </summary>
        public List<TimetableClassRequest> Classes { get; set; }
    }

    /// <summary>
    /// 课程表的班级信息
    /// </summary>
    public class TimetableClassRequest
    {
        /// <summary>
        /// 班级课表主健
        /// </summary>
        [Required]
        public long AutClassId { get; set; }
        /// <summary>
        /// 班级代码
        /// </summary>
        [Required]
        public string ClassNo { get; set; }

        /// <summary>
        /// 教师ID
        /// </summary>
        [Required]
        public string TeacherId { get; set; }
        /// <summary>
        /// 课次
        /// </summary>
        [Required]
        public int CourseNum { get; set; }

        /// <summary>
        /// 学位数
        /// </summary>
        [Required]
        public int StudentsNum { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [Required]
        public long CourseId { get; set; }
        /// <summary>
        /// 课程级别ID
        /// </summary>
        [Required]
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 上课时间段主健
        /// </summary>
        [Required]
        public List<long> SchoolTimeId { get; set; }
    }
}
