using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 交接状态
    /// </summary>
    public enum TransferStatus
    {
        /// <summary>
        /// 未交接
        /// </summary>
        [Description("未交接")]
        UnTransfer = 0,
        /// <summary>
        /// 已交接
        /// </summary>
        [Description("已交接")]
        HasTransfer = 1
    }
}
