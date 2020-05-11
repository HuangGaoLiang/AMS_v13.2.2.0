using System;

namespace AMS.Models
{
    /// <summary>
    /// 报名作废月
    /// </summary>
    public class ViewOrderObsoleteMonth
    {
        /// <summary>
        /// 主键
        /// </summary>

        public long EnrollOrderId { get; set; }

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
        /// 监护人手机号码
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal TotalTradeAmount { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal UseBalance { get; set; }

        /// <summary>
        /// 交易总额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 付款方式（1刷卡 2现金 3转账 4微信 5支付宝 9其他）
        /// </summary>
        public int PayType { get; set; }

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

        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
    }
}
