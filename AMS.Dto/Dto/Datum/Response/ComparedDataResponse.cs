using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 对比数据实体集
    /// </summary>
    public class ComparedDataResponse
    {
        /// <summary>
        /// 班级课表id
        /// </summary>
        public string AutClassId { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        public string AuditId { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolNo { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }
        /// <summary>
        /// 教室表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RoomCourseId { get; set; }
        /// <summary>
        /// 教室Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }
        /// <summary>
        /// 课程等级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLeveId { get; set; }
        /// <summary>
        /// 课次
        /// </summary>
        public int CourseNum { get; set; }
        /// <summary>
        /// 学生数量/学位
        /// </summary>
        public int StudentsNum { get; set; }
        /// <summary>
        /// 上课老师Id
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 数据状态 -1：删除 0：不变 1：增加
        /// </summary>
        public DataStatus DataStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
