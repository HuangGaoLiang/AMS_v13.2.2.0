using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 优惠券类型
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 全部(此字段与数据库无关)
        /// </summary>
        [Description("全部")]
        All = 0,
        /// <summary>
        /// 满减
        /// </summary>
        [Description("满减")]
        FullReduce = 1,
        /// <summary>
        /// 转介绍
        /// </summary>
        [Description("转介绍")]
        Recommend = 2,
        /// <summary>
        /// 校长奖学金
        /// </summary>
        [Description("校长奖学金")]
        HeadmasterBonus = 3

    }
}
