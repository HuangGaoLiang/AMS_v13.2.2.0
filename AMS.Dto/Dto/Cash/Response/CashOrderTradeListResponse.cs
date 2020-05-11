using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：缴费交易记录实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    public class CashOrderTradeListResponse
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// 交易单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 交易类型Id
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeTypeName { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 交易Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

        /// <summary>
        /// 缴费类型
        /// </summary>
        public string Remark { get; set; }
    }
}
