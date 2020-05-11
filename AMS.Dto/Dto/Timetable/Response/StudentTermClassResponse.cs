using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 学生所上课的学期数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class StudentTermResponse
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期类型Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }
        /// <summary>
        /// 学期类型姓名
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 学期Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 学生所上课的班级数据
        /// </summary>
        public List<StudentClassResponse> StudentClassList { get; set; }
    }


    /// <summary>
    /// 学生所上课的班级数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class StudentClassResponse
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }
    }
}
