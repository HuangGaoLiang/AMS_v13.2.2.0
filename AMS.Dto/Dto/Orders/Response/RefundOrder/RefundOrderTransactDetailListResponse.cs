using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学和退班、转班都使用此公共实体
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class RefundOrderTransactDetailListResponse
    {

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期类型Id
        /// </summary>
        public long TermTypeId { get; set; }

        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }
        /// <summary>
        /// 报名订单课程明细Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 课程级别Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseLevelId { get; set; }
        /// <summary>
        /// 课程级别名称
        /// </summary>
        public string CourseLevelName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 上课课次
        /// </summary>
        public int HaveClassLesson { get; set; }
        /// <summary>
        /// /扣除费用
        /// </summary>
        public decimal DeductAmount { get; set; }
        /// <summary>
        /// 休学课次/退班课次
        /// </summary>
        public int LeaveSchoolLessons { get; set; }

        /// <summary>
        /// 存入金额（退费金额）
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 学费单价(原价)
        /// </summary>
        public decimal TuitionFee { get; set; }
        /// <summary>
        /// 杂费单价(原价)
        /// </summary>
        public decimal MaterialFee { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountFee { get; set; }

        /// <summary>
        /// 课程订单状态
        /// </summary>
        public OrderItemStatus Status { get; set; }
    }
}
