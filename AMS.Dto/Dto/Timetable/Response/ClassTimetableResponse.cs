using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{

    /// <summary>
    /// 一个班级的课程表详细信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassTimetableResponse
    {
        /// <summary>
        /// 第一节课的上课与下课时间,Time1[0] 上课时间 Time1[0] 下课时间
        /// </summary>
        public List<string> Time1 { get; set; }

        /// <summary>
        /// 当IsClassJoin=true时，代表第二节课的上课与下课时间
        /// Time2[0] 上课时间 Time2[0] 下课时间
        /// </summary>
        public List<string> Time2 { get; set; }

        /// <summary>
        /// 星期几与上课时间的数据
        /// </summary>
        public List<ClassTimetableSchoolTimeResponse> WeekDaySchoolTimes { get; set; }

        /// <summary>
        /// 课程与课程级别
        /// </summary>
        public List<ClassTimetableCourseResponse> ClassTimetableCourse { get; set; }

        /// <summary>
        /// 上课老师
        /// </summary>
        public List<ClassTimetableTeacherResponse> Teacher { get; set; }

        /// <summary>
        /// 是否可以与下一节连着上
        /// </summary>
        public bool CanClassJoin { get; set; }

        /// <summary>
        /// 是否与下一个时间段连上
        /// </summary>
        public bool IsClassJoin { get; set; }

        /// <summary>
        /// 选中的课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; } = 0;

        /// <summary>
        /// 选中的课程等级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; } = 0;

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; } = string.Empty;

        /// <summary>
        /// 选中的老师Id
        /// </summary>
        public string TeaherId { get; set; } = string.Empty;

        /// <summary>
        /// 课次
        /// </summary>
        public int CourseNum { get; set; } = 0;

        /// <summary>
        /// 学位数量
        /// </summary>
        public int StudentsNum { get; set; } = 0;

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }
    }


    /// <summary>
    /// 星期几与上课时间的数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassTimetableSchoolTimeResponse
    {
        /// <summary>
        /// 星期几
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 第一节上课时间ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId1 { get; set; }

        /// <summary>
        /// 是否包含第一节课上课时间
        /// </summary>
        public bool HasSchoolTime1 { get; set; }

        /// <summary>
        /// 是否包含第二节课上课时间
        /// </summary>
        public bool HasSchoolTime2 { get; set; }

        /// <summary>
        /// 第二节上课时间ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId2 { get; set; }
    }


    /// <summary>
    /// 课程
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassTimetableCourseResponse
    {
        /// <summary>
        /// 课程主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程代码
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 课程中文名
        /// </summary>
        public string CourseCnName { get; set; }

        /// <summary>
        /// 课程级别
        /// </summary>
        public List<ClassTimetableCourseLevelResponse> Levels { get; set; }

    }


    /// <summary>
    /// 课程的级别
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassTimetableCourseLevelResponse
    {
        /// <summary>
        /// 主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 课程级别代码
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string LevelCnName { get; set; }
    }

    /// <summary>
    /// 描    述：上课老师
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-12-11</para>
    /// </summary>
    public class ClassTimetableTeacherResponse
    {
        /// <summary>
        /// 上课老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 上课老师编号
        /// </summary>
        public string TeacherId { get; set; }
    }
}
