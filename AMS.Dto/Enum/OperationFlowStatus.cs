using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 操作日志状态
    /// </summary>
    public enum OperationFlowStatus
    {
        /// <summary>
        /// 已拒绝
        /// </summary>
        [Description("拒绝")]
        Refuse = -2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("撤销")]
        Cancel = -1,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit = 0,
        /// <summary>
        /// 已确认
        /// </summary>
        [Description("确认")]
        Confirm = 1,
        /// <summary>
        /// 已接收
        /// </summary>
        [Description("接收")]
        Finish = 10
    }
}
