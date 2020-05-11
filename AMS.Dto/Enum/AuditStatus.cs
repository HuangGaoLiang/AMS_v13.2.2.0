using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 审核状态
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-13</para>
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 保存待提交审核
        /// </summary>
        [Description("保存待提交审核")]
        WaitAudit =0,
        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审核中")]
        Auditing = 1,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Success = 2,

        /// <summary>
        /// 退回
        /// </summary>
        [Description("退回")]
        Return = 3,
        ///// <summary>
        ///// 退回  
        ///// </summary>
        //[Description("退回")]
        //Return = 4,
        /// <summary>
        /// 转发
        /// </summary>
        [Description("转发")]
        Forwarding = 5,
        ///// <summary>
        ///// 撤销
        ///// </summary>
        //[Description("撤销")]
        //Undo =6


    }
}
