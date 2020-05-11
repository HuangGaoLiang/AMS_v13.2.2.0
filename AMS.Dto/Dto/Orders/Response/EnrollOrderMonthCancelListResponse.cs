using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  报名订单当月可取消列表
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderMonthCancelListResponse
    {
        /// <summary>
        /// 主键(报名订单)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string ContactPersonMobile { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalance { get; set; }

        /// <summary>
        /// 交易总额
        /// </summary>
        public decimal TotalTradeAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayTypeId { get; set; }

        /// <summary>
        /// 付款方式  （1：刷卡，2，：现金，3：银行转账，4：微信支付，5：支付宝支付，9：其他）
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 订单状态(-1:已取消,0:待付款,-1:已付款,10:完成)
        /// </summary>
        public string OrderStatus { get; set; }
    }
}
