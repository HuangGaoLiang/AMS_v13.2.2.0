using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 校区课程总表(已生效)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TermTimetableResponse
    {
        /// <summary>
        /// 60分钟课程表
        /// </summary>
        public TermCourseTimetableResponse CourseTimetableSixty { get; set; }
        /// <summary>
        /// 90分钟课程表
        /// </summary>
        public TermCourseTimetableResponse CourseTimetableNinety { get; set; }
        /// <summary>
        /// 审核信息
        /// </summary>
        public TermTimetableAuditResponse TermTimetableAuditResponse { get; set; }
        /// <summary>
        /// 学期开始时间
        /// </summary>
        public string TermBeginDate { get; set; }
        /// <summary>
        /// 学期结束时间
        /// </summary>
        public string TermEndDate { get; set; }
    }

    /// <summary>
    /// 审核信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TermTimetableAuditResponse
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus AuditStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string Auditime { get; set; }

        /// <summary>
        /// 是否第一次提交人
        /// </summary>
        public bool IsFirstSubmitUser { get; set; } = true;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
    }




    /// <summary>
    /// 课程表明细
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TermCourseTimetableResponse
    {
        /// <summary>
        /// 课程表的教室
        /// </summary>
        public List<TimetableClassRoomResponse> ClassRooms { get; set; }
        /// <summary>
        /// 课程表的时间信息
        /// </summary>
        public List<TimetableSchoolTimeResponse> SchoolTimes { get; set; }
    }

    /// <summary>
    /// 课程表的教室
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TimetableClassRoomResponse
    {
        /// <summary>
        /// 教室Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 课程表的班级信息
        /// </summary>
        public List<TimetableClassResponse> Classes { get; set; }
    }

    /// <summary>
    /// 课程表的班级信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TimetableClassResponse
    {
        /// <summary>
        /// 班级主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string CourseLevelName { get; set; }

        /// <summary>
        /// 课程等级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLeveId { get; set; }

        /// <summary>
        /// 上课时间段主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 课次索引 如周一的第一节课=1
        /// </summary>
        public int LessonIndex { get; set; }

        /// <summary>
        /// 教师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; }
    }

    /// <summary>
    /// 课程表的时间表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TimetableSchoolTimeResponse
    {
        /// <summary>
        /// 星期几
        /// </summary>
        public string WeekDay { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public List<SchoolTimePeriodResponse> Times { get; set; }
    }

    /// <summary>
    /// 时间段
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class SchoolTimePeriodResponse
    {
        /// <summary>
        /// 时间段Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }

}
