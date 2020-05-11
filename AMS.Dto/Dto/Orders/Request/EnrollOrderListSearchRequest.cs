using Jerrisoft.Platform.Public.PageExtensions;
using System;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  报班订单列表查询条件
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderListSearchRequest : Page,IOrderListSearchRequest
    {
        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 单据状态 -2：拒绝 -1：订单取消 0：待付款 1：已确认 9：已付款/正常 10：已完成
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }

        /// <summary>
        /// 收款方式 1：刷卡 2：现金 3：银行转账 4：微信支付 5：支付宝支付 9：其他
        /// </summary>
        public PayType? PayType { get; set; }

        /// <summary>
        /// 收款开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 收款结束日期
        /// </summary>
        public DateTime? EndtTime { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string Cashier { get; set; }

        /// <summary>
        /// 学生信息:学生姓名或者监护人手机号
        /// </summary>
        public string StudentInfo { get; set; }

        /// <summary>
        /// 新生/老生
        /// </summary>
        public OrderNewType? OrderNewType { get; set; }

    }
}
