using System;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 停课时间段数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class SchoolHolidayResponse
    {
        /// <summary>
        /// 停课日Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long HolidayId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime STime { get; set; }

        /// <summary>
        /// 开始时间是否冻结
        /// </summary>
        public bool STimeIsFreeze => STime <= DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ETime { get; set; }

        /// <summary>
        /// 结束时间是否冻结
        /// </summary>
        public bool ETimeIsFreeze => ETime <= DateTime.Now;

        /// <summary>
        /// 时间差距
        /// </summary>
        public int TimeGap => (ETime - STime).Days + 1;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
