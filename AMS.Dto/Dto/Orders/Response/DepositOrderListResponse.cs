using AMS.Core;
using Newtonsoft.Json;
using System;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订金列表信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class DepositOrderListResponse : IOrderListResponse
    {
        /// <summary>
        /// 订金主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long DepositOrderId { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 付款方式（1刷卡 2现金 3转账 4微信 5支付宝 9其他）
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 付款方式名称
        /// </summary>
        public string PayTypeName { get; set; }

        /// <summary>
        /// 订金用途（1预报名2游学营定金3写生定金）
        /// </summary>
        public int UsesType { get; set; }

        /// <summary>
        /// 订金用途
        /// </summary>
        public string UsesTypeName { get; set; }

        /// <summary>
        /// 收银人编号
        /// </summary>
        public string PayeeId { get; set; }

        /// <summary>
        /// 收银人名称
        /// </summary>
        public string Payee { get; set; }
        /// <summary> 
        /// -1订单取消, 0待付款,1已付款/正常 10 已完成
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary> 
        /// 订单状态
        /// </summary>
        public string OrderStatusName { get; set; }
    }
}
