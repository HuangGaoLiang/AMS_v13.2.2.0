using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 退款方式
    /// </summary>
    public enum RefundType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 退给家长
        /// </summary>
        [Description("退给家长")]
        RefundToParent = 1,
        /// <summary>
        /// 退到余额
        /// </summary>
        [Description("退到余额")]
        RefundToBalance = 2
    }
}
