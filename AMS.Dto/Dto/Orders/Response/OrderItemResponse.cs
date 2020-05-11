using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订单明细
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class OrderItemResponse
    {
        /// <summary>
        /// 主健(报名订单课程明细)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 报名的排课信息
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        [JsonIgnore]
        public int ClassTimes { get; set; }
    }
}
