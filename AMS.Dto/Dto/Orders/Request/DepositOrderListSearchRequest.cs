using Jerrisoft.Platform.Public.PageExtensions;
using System;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订金查询
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class DepositOrderListSearchRequest : Page,IOrderListSearchRequest
    {
        /// <summary>
        /// 校区编号
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 单据状态 
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }

        /// <summary>
        /// 付款方式 1刷卡 2现金 3转账 4微信 5支付宝 9其他
        /// </summary>
        public PayType? PayType { get; set; }

        /// <summary>
        /// 订金用途 1预报名2游学营定金3写生定金
        /// </summary>
        public UsesType? UsesType { get; set; }

        /// <summary>
        /// 收款开始日期
        /// </summary>
        public DateTime? StartPayDate { get; set; }

        /// <summary>
        /// 收款结束日期
        /// </summary>
        public DateTime? EndPayDate { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// 学生信息 学生姓名或者手机号码
        /// </summary>
        public string StudentInfo { get; set; }
    }
}
