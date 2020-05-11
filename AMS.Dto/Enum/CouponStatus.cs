using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto.Enum
{
    /// <summary>
    /// 优惠券状态状态
    /// </summary>
    public enum CouponStatus : int
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,
        /// <summary>
        /// 作废
        /// </summary>
        Invalid = -1,
        /// <summary>
        /// 未使用
        /// </summary>
        [Description("未使用")]
        NoUse = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        HasUse = 2,
    }
}


