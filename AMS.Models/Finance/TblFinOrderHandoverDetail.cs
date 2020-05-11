/*此代码由生成工具字段生成，生成时间17/11/2018 20:55:48 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// TblFinOrderHandoverDetail(订单交接核对明细)
    /// </summary>
    public partial class TblFinOrderHandoverDetail
    {
        /// <summary>
        /// 主健(订单交接核对明细)
        /// </summary>
        public long OrderHandoverDetailId { get; set; }

        /// <summary>
        /// 所属核对主表
        /// </summary>
        public long OrderHandoverId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 招生专员ID
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalanceAmount { get; set; }

        /// <summary>
        /// 优惠总额
        /// </summary>
        public decimal TotalDiscountFee { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? HandoverStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime? HandoverDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName { get; set; }

    }
}
