/*此代码由生成工具字段生成，生成时间2018/11/1 14:45:49 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 订金表
    /// </summary>
    public partial class TblOdrDepositOrder
    {
        /// <summary>
        /// 主健（DepositOrderId）
        /// </summary>
        public long DepositOrderId { get; set; }

        /// <summary>
        /// 学生编号（TblCstStudent）
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 归属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 订金用途(1预报名2游学营定金3写生定金)
        /// </summary>
        public int UsesType { get; set; }

        /// <summary>
        /// 定金
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收银人编号
        /// </summary>
        public string PayeeId { get; set; }

        /// <summary>
        /// 收银人
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// 1刷卡 2现金 3转账 4微信 5支付宝 9其他
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 单据状态（-1订单取消, 0待付款,1已付款/正常 10完成）
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 作废操作人
        /// </summary>
        public string CancelUserId { get; set; }

        /// <summary>
        /// 作废操作名称
        /// </summary>
        public string CancelUserName { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelRemark { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
