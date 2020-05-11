using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  报班收款详情
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderItemResponse
    {
        /// <summary>
        /// 主健(报名订单课程明细)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 学期类型ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 学期课次
        /// </summary>
        public int ClassesCount { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类型(0必修,1选修)
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 课程类型名称
        /// </summary>
        public string CourseTypeName { get; set; }

        /// <summary>
        /// 课程级别ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 课程级别名称
        /// </summary>
        public string CourseLeveName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }


    }
}
