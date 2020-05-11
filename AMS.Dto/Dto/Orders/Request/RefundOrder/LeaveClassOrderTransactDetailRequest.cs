using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退班订单详情
    /// <para>作   者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderTransactDetailRequest : IRefundOrderTransacDetailtRequest
    {
        /// <summary>
        /// 退班开始日期
        /// </summary>
        public DateTime LeaveTime { get; set; }
        
    }
}
