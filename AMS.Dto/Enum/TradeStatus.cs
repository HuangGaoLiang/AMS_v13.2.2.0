using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum TradeStatus
    {
        /// <summary>
        /// 交易取消
        /// </summary>
        Cancel = -1,
        /// <summary>
        /// 交易中
        /// </summary>
        Trading = 1,
        /// <summary>
        /// 交易完成
        /// </summary>
        Complete = 2
    }
}
