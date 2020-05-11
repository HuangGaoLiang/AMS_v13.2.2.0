using System;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 转校列表(转出和转入列表共用数据模型)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderListResponse : IOrderListResponse
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 转出日期/转入日期
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatusName { get; set; }
    }
}
