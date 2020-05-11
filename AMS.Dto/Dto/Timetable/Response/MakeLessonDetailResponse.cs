using System;
using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 学生报名一个课程的排课详细信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class MakeLessonDetailResponse
    {
        /// <summary>
        /// 报名信息
        /// </summary>
        public RegisterInformation RegisterInfo { get; set; }

        /// <summary>
        /// 排课信息
        /// </summary>
        public List<CourseInformation> CourseInfos { get; set; }
    }

    /// <summary>
    /// 报名信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class RegisterInformation
    {
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime EnrollDate { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期类型Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }

        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public CourseType CourseType { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 已排课次
        /// </summary>
        public int ClassTimesUse { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int ClassTimesRD => ClassTimes - ClassTimesUse;
    }

    /// <summary>
    /// 排课信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseInformation
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public List<string> Week { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public List<string> PeriodTime { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 首次上课时间
        /// </summary>
        public DateTime FirstClassTime { get; set; }

        /// <summary>
        /// 已排课次
        /// </summary>
        public int ClassTimesUse { get; set; }
    }
}
