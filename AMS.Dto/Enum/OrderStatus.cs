
using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 订单状态
    /// 2018-10-24 蔡亚康
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 已拒绝
        /// </summary>
        [Description("已拒绝")]
        Refuse = -2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已作废")]
        Cancel = -1,
        /// <summary>
        /// 待付款
        /// </summary>
        [Description("待付款")]
        WaitPay = 1,
        /// <summary>
        /// 已确认
        /// </summary>
        [Description("已确认")]
        Confirm = 2,
        /// <summary>
        /// 已付款/正常
        /// </summary>
        [Description("未交接")]
        Paid = 9,
        /// <summary>
        /// 完成/已交接/已接收
        /// </summary>
        [Description("完成")]
        Finish = 10
    }
}
