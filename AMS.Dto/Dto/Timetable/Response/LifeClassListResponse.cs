using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassListResponse
    {
        /// <summary>
        /// 写生课Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LifeTimeId { get; set; }

        /// <summary>
        /// 写生课代码
        /// </summary>
        public string LifeClassCode { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 写生地点
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime ClassBeginDate { get; set; }

        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime ClassEndTime { get; set; }

        /// <summary>
        /// 消耗课次
        /// </summary>
        public int UseLessonCount { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PersonNumber { get; set; }
    }
}
