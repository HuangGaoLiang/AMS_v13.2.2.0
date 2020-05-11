using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Models
{
    /// <summary>
    /// 描述：休学订单信息列表
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ViewLeaveSchoolOrder
    {
        /// <summary>
        /// 退费订单表主键
        /// </summary>
        public long RefundOrderId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 休学日期
        /// </summary>
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }
        /// <summary>
        /// 休学课次
        /// </summary>
        public int TotalRefundLessonCount { get; set; }
        /// <summary>
        /// 存入金额（退费金额）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 休学原因
        /// </summary>
        public string Reason { get; set; }
    }
}
