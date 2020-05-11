using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退费订单信息
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class OdrRefundOrderResponse
    {
        /// <summary>
        /// 主键(退费订单-休学)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RefundOrderId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int RemindClassTimes { get; set; }

        /// <summary>
        /// 休学日期
        /// </summary>
        public DateTime LeaveTime { get; set; }

        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }

        /// <summary>
        /// 学生状态
        /// </summary>
        public int StudyStatus { get; set; }


    }
}
