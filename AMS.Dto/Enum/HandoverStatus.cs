using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 订单交接核对状态
    /// </summary>
    public enum HandoverStatus
    {
        /// <summary>
        /// 问题单
        /// </summary>
        [Description("问题单")]
        Problematic = -1,
        /// <summary>
        /// 已核对
        /// </summary>
        [Description("已核对")]
        Checked = 1,
        /// <summary>
        /// 已交接
        /// </summary>
        [Description("已交接")]
        Handover = 2
    }
}
