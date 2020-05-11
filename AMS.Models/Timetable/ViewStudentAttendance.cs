using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生考勤实体类
    /// </summary>
    public class ViewStudentAttendance
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long? StudentId { get; set; }

        /// <summary>
        /// 学生课次Id
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 学生考勤课程Id
        /// </summary>
        public long? StudentLessonId { get; set; }

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
        /// 上课日期
        /// </summary>
        public DateTime ClassBeginDate { get; set; }

        /// <summary>
        /// 下课日期
        /// </summary>
        public DateTime ClassEndDate { get; set; }

        /// <summary>
        /// 课程类型1常规课2写生课
        /// </summary>
        public int LessonType { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassLessonCount { get; set; }

        /// <summary>
        /// 耗用课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 签到人员类型1老师9财务
        /// </summary>
        public int? AttendUserType { get; set; }

        /// <summary>
        /// 考勤老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 班级老师Id
        /// </summary>
        public string ClassTeacherId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int? AttendStatus { get; set; }

        /// <summary>
        /// 课程状态
        /// </summary>
        public int LessonStatus { get; set; }

        /// <summary>
        /// 补课/调课类型0常规课1已经安排补课2已安排调课,
        /// </summary>
        public int AdjustType { get; set; }

        /// <summary>
        /// 考勤时间
        /// </summary>
        public DateTime? AttendDate { get; set; }

        /// <summary>
        /// 课程业务类型
        /// </summary>
        public int? BusinessType { get; set; }

        /// <summary>
        /// 课程处理状态
        /// </summary>
        public int ProcessStatus { get; set; }
    }
}
