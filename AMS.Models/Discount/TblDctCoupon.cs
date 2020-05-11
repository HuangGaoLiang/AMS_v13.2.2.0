/*此代码由生成工具字段生成，生成时间2018/11/1 14:45:49 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 优惠券表
    /// </summary>
    public partial class TblDctCoupon
    {
        /// <summary>
        /// 主健
        /// </summary>
        public long CouponId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 券号
        /// </summary>
        public string CouponNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int CouponType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否全免 (校长奖学金类型的优惠券时用,选择全免为true,没有选则为false，其余类型的优惠券为fasle)
        /// </summary>
        public bool IsFreeAll { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime { get; set; }
        /// <summary>
        /// 来源ID
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// 来源订单
        /// </summary>
        public long EnrollOrderId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
