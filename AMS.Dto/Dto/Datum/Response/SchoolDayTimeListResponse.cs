using System;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:某天的上课时间段信息 
    /// <para>作    者:  瞿琦</para>
    /// <para>创建时间: 2019-3-14</para>
    /// </summary>
    public class SchoolDayTimeListResponse
    {
        /// <summary>
        /// 上课时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
