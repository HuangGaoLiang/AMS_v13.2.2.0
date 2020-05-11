using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 附件类型
    /// </summary>
    public enum AttchmentType
    {
        /// <summary>
        /// 休学办理-上传申请表
        /// </summary>
        LEAVE_SCHOOL_APPLY,
        /// <summary>
        /// 退班办理-上传申请表
        /// </summary>
        LEAVE_CLASS_APPLY,
        /// <summary>
        /// 订单交接附件
        /// </summary>
        ORDER_HAND_OVER,
        /// <summary>
        /// 转校
        /// </summary>
        ORDER_TRANSFERSCHOOL,
        /// <summary>
        /// 余额退费
        /// </summary>
        ORDER_BALANCE_REFUND,
        /// <summary>
        /// 投诉与建议
        /// </summary>
        FEEDBACK,
    }
}
