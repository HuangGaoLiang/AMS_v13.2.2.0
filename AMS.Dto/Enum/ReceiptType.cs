using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto.Enum
{
    /// <summary>
    /// 票据类型
    /// ---瞿琦  2018-11-8
    /// </summary>
    public enum ReceiptType
    {
        /// <summary>
        /// 未开发票
        /// </summary>
        [Description("未开发票")]
        NoGiveReceipt =1,
        /// <summary>
        /// 发票齐全
        /// </summary>
        [Description("发票齐全")]
        ReceiptComplete =2,
        /// <summary>
        /// 发票遗失
        /// </summary>
        [Description("发票遗失")]
        LostReceipt = 3
    }
}
