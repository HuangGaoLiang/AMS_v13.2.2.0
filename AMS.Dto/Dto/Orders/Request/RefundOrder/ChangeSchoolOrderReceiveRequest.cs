using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 转校接收
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderReceiveRequest
    {
        /// <summary>
        /// 操作人Id
        /// </summary>
        [JsonIgnore]
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [JsonIgnore]
        public string OperatorName { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 接收状态 1:接收 -1:拒绝
        /// </summary>
        public ChangeSchoolOrderReceiveStatus ReceiveStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
