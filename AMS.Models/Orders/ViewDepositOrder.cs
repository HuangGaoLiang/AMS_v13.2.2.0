using System;

namespace AMS.Models
{
    /// <summary>
    /// 订金列表
    /// </summary>
    public class ViewDepositOrder
    {
        /// <summary>
        /// 订金主键
        /// </summary>

        public long DepositOrderId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 学生ID
        /// </summary>

        public long StudentId { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 付款方式（1刷卡 2现金 3转账 4微信 5支付宝 9其他）
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 订金用途（1预报名2游学营定金3写生定金）
        /// </summary>
        public int UsesType { get; set; }

        /// <summary>
        /// 收银人编号
        /// </summary>
        public string PayeeId { get; set; }

        /// <summary>
        /// 收银人名称
        /// </summary>
        public string Payee { get; set; }

        /// <summary> 
        /// -1订单取消, 0待付款,1已付款/正常 10 已完成
        /// </summary>
        public int OrderStatus { get; set; }
    }
}
