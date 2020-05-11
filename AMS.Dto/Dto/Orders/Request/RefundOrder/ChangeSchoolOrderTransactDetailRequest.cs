using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 转校订单详情(转校订单创建前)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderTransactDetailRequest : IRefundOrderTransacDetailtRequest
    {
        /// <summary>
        /// 转校时间
        /// </summary>
        public DateTime ChangeSchoolTime { get; set; }
    }
}
