using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 本校转入
    /// </summary>
    public enum OrderStatusTransferResponse
    {
        /// <summary>
        /// 已拒绝
        /// </summary>
        [Description("已拒绝")]
        Refuse = -2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已撤销")]
        Cancel = -1,

        /// <summary>
        /// 待确认
        /// </summary>
        [Description("待确认")]
        WaitPay = 1,
        /// <summary>
        /// 待接收
        /// </summary>
        [Description("待接收")]
        Confirm = 2,
        /// <summary>
        /// 已接收
        /// </summary>
        [Description("已接收")]
        Paid = 9,
        /// <summary>
        /// 完成/已交接/已接收
        /// </summary>
        [Description("完成")]
        Finish = 10
    }
}
