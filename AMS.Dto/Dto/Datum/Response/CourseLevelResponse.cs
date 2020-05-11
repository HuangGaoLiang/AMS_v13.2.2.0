using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  课程级别数据，用在列表或详情数据
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class CourseLevelResponse
    {
        /// <summary>
        /// 课程级别主健(TblDatCourseLevel)
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

        /// <summary>
        /// 英文名
        /// </summary>
        public string LevelEnName { get; set; }

        /// <summary>
        /// 停用/启用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
