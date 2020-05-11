using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：收款交接表信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-19</para>
    /// </summary>
    public class OrderHandoverReportResponse
    {
        /// <summary>
        /// 招生专员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 当日已收款当日入系统(全部)的合计金额
        /// </summary>
        public decimal TradeTotalAmount { get; set; }

        /// <summary>
        /// 往日已收款今日入系统(本月)的已核对的订单的合计金额
        /// </summary>
        public decimal CurrentMonthTotalAmount { get; set; }

        /// <summary>
        /// 往日已收款今日入系统(往月)的合计金额
        /// </summary>
        public decimal LastMonthTotalAmount { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 所有已核对的订单的合计金额
        /// </summary>
        public decimal TradeSummaryTotalAmount { get; set; }

        /// <summary>
        /// 收现钞
        /// </summary>
        public decimal DayIncomeAmout { get; set; }

        /// <summary>
        /// 存入银行现钞
        /// </summary>
        public decimal InBankAmount { get; set; }

        /// <summary>
        /// 存单张数
        /// </summary>
        public decimal ReceiptNumber { get; set; }

        /// <summary>
        /// 招生专员签字
        /// </summary>
        public string HandoverUrl { get; set; }

        /// <summary>
        /// 驻校出纳签字
        /// </summary>
        public string RecipientUrl { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        public DateTime HandoverDate { get; set; }

        /// <summary>
        /// 差异说明
        /// </summary>
        public string Remark { get; set; }        

        /// <summary>
        /// 支付日期等于创建日期的已核对的订单
        /// </summary>
        public List<OrderHandoverTradeResponse> TradeList { get; set; }

        /// <summary>
        /// 往日已收款今日入系统(本月)的已核对的订单
        /// </summary>
        public List<OrderHandoverDaySummaryResponse> CurrentMonthList { get; set; }

        /// <summary>
        /// 往日已收款今日入系统(往月)的已核对的订单
        /// </summary>
        public List<OrderHandoverDaySummaryResponse> LastMonthList { get; set; }

        /// <summary>
        /// 所有已核对的订单
        /// </summary>
        public List<OrderHandoverTradeSummaryResponse> TradeSummaryList { get; set; }
    }
}
