namespace AMS.Storage.Models
{
    /// <summary>
    /// 报名订单优惠
    /// </summary>
    public partial class TblOdrEnrollOrderDiscount
    {
        /// <summary>
        /// 主健(报名订单优惠)
        /// </summary>
        public long EnrollOrderDiscountId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 所属订单
        /// </summary>
        public long EnrollOrderId { get; set; }
        /// <summary>
        /// 使用的优惠券ID
        /// </summary>
        public long CouponId { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 优惠券类型（1：转介绍，2 ：满减奖学金，3：校长奖学金）
        /// </summary>
        public int CouponType { get; set; }
    }
}
