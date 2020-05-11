using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 余额交易列表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class WalletTradeListResponse
    {
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal TransAftBalance { get; set; }

        /// <summary>
        /// 支出
        /// </summary>
        public decimal Expenditure { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime? TransDate { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        public string Remark { get; set; }
    }
}
