using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 余额冻结状态
    /// </summary>
    public enum BalanceFrozeStatus
    {
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Cancel = -1,
        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Process = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finish = 10
    }
}
