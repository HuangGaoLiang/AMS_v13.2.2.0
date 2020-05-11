using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学期类型的详细信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-05</para>
    /// </summary>
    public class StudyPlanTermItemResponse
    {
        /// <summary>
        /// 学期类型Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程级别Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 0必修/1选修
        /// </summary>
        public CourseType CourseType { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 课程级别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 课长
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 单次课学费
        /// </summary>
        public int TuitionFee { get; set; }

        /// <summary>
        /// 单次课杂费
        /// </summary>
        public int MaterialFee { get; set; }
    }
}
