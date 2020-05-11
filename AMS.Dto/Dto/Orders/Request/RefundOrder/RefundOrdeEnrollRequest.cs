using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退费订单--退的课程明细
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class RefundOrdeEnrollRequest
    {
        /// <summary>
        /// 报名订单课程明细ID
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 退费课次/休学课次/转校课次  数量
        /// </summary>
        public long LessonCount { get; set; }
    }
}
