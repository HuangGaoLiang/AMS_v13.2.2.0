using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学办理-费用明细
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class EnrollOrderResponse
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期类型
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程级别ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 已排课次
        /// </summary>
        public int ClassTimesUse { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 学费单价(原价)
        /// </summary>
        public decimal TuitionFee { get; set; }
        /// <summary>
        /// 杂费单价(原价)
        /// </summary>
        public decimal MaterialFee { get; set; }

        /// <summary>
        /// 学费单价(实价)
        /// </summary>
        public decimal TuitionFeeReal { get; set; }
        /// <summary>
        /// 杂费单价(实价)
        /// </summary>
        public decimal MaterialFeeReal { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountFee { get; set; }

        /// <summary>
        /// 课程订单状态
        /// </summary>
        public OrderItemStatus Status { get; set; }
    }
}
