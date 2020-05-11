using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.Enum
{
    /// <summary>
    /// 赠与奖学金状态
    /// </summary>
    public enum CouponRuleStatus:byte
    {
        /// <summary>
        /// 全部
        /// </summary>
        All=0,
        /// <summary>
        /// 生效  ---赠与奖学金
        /// </summary>
        Effect = 1,
        /// <summary>
        /// 待生效  ---赠与奖学金
        /// </summary>
        WaitEffect = 3,

        /// <summary>
        /// 失效  ---赠与奖学金
        /// </summary>
        NoEffect = 2,
        
    }
}
