using AMS.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 班级详情
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassDetailResponse
    {
        /// <summary>
        /// 班级表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 学期编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RoomCourseId { get; set; }

        /// <summary>
        /// 教室Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string TeachName { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassRoomName { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public string CourseName { get; set; }

        /// <summary>
        /// 课程等级
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLeveId { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string CourseLeveName { get; set; }

        /// <summary>
        /// 课次 
        /// </summary>
        public int CourseNum { get; set; }
        /// <summary>
        /// 班级上课时间课表
        /// </summary>
        public List<SchoolTime> SchoolTimeList { get; set; }

        /// <summary>
        /// 上课时间段（拼接）
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 学生数量/学位
        /// </summary>
        public int StudentsNum { get; set; }

        /// <summary>
        /// 剩余学位数
        /// </summary>
        public int SurplusNum { get; set; }
    }

    /// <summary>
    /// 下课时间
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class SchoolTime
    {
        /// <summary>
        /// 主健
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long SchoolTimeId { get; set; }

        /// <summary>
        /// 星期几
        /// </summary>
        public int WeekDay { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 上课时间（拼接好的格式）
        /// </summary>
        public string ClassTime { get; set; }
    }
}
