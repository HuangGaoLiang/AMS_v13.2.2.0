using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 报班退费明细
    /// </summary>
    public class ViewLeaveClassDetail
    {
        /// <summary>
        /// 退费订单主键
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        ///// <summary>
        ///// 退费时间
        ///// </summary>
        //public DateTime RefundDate { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string LinkMobile { get; set; }
        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退费方式
        /// </summary>
        public int? RefundType { get; set; }
        /// <summary>
        /// 票据状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单退费时间
        /// </summary>
        public DateTime OrderRefundTime { get; set; }
    }
}
