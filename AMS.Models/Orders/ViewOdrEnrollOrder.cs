using System;

namespace AMS.Models
{
    /// <summary>
    /// 报名订单
    /// </summary>
    public class ViewOdrEnrollOrder
    {
        /// <summary>
        /// 报名主键编号
        /// </summary>
        public long EnrollOrderId { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 新生/老生
        /// </summary>
        public int OrderNewType { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 付款总额（实收金额）
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int PayTypeId { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public int OrderStatusId { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalance { get; set; }

        /// <summary>
        /// 付款总额
        /// </summary>
        public decimal TotalTradeAmount { get; set; }


    }
}
