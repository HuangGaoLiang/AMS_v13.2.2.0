using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：班级学生人数实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class TimeClassStudentResponse
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 学期Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int PersonNumber { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string WeekTimes { get; set; }
    }
}
