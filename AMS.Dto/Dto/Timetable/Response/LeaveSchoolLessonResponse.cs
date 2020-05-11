using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 休学课次
    /// </summary>
    public class LeaveSchoolLessonResponse
    {
        /// <summary>
        /// 报名课程订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 休学课次
        /// </summary>
        public int Count { get; set; }
    }
}
