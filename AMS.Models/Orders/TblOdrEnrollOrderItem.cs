namespace AMS.Storage.Models
{
    /// <summary>
    /// 报名订单课程明细
    /// </summary>
    public partial class TblOdrEnrollOrderItem
    {
        /// <summary>
        /// 主健(报名订单课程明细)
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public long EnrollOrderId { get; set; }
        /// <summary>
        /// 报名方式(1报名课程 2 报名学期类型)
        /// </summary>
        public int EnrollType { get; set; }
        /// <summary>
        /// 学期类型ID
        /// </summary>
        public long TermTypeId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 课程类型(0必修1选修)
        /// </summary>
        public int CourseType { get; set; }
        /// <summary>
        /// 课程级别ID
        /// </summary>
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 课程时间
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }
        /// <summary>
        /// 已排课次
        /// </summary>
        public int ClassTimesUse { get; set; }
        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal DiscountFee { get; set; }
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
        /// 状态(1:已报名,2:已退费,3:已休学,4:已转校,5:已作废)
        /// </summary>
        public int Status { get; set; }
    }
}
