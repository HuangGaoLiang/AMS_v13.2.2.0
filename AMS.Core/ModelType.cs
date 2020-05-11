using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 业务模块类型
    /// </summary>
    public enum ModelType : byte
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,
        /// <summary>
        /// 基础配置模块
        /// </summary>
        Datum = 1,
        /// <summary>
        /// 课程计划模块
        /// </summary>
        Timetable = 2,
        /// <summary>
        /// 审核模块
        /// </summary>
        Audit = 3,
        /// <summary>
        /// 报名模块
        /// </summary>
        SignUp = 4,
        /// <summary>
        /// 优惠券模块
        /// </summary>
        Discount = 5,
        /// <summary>
        /// 资金交易模块
        /// </summary>
        Cash = 7,
        /// <summary>
        /// 客户模块
        /// </summary>
        Customer = 8,
        /// <summary>
        /// 订单模块
        /// </summary>
        Order = 9,
        /// <summary>
        /// 家校互联模块
        /// </summary>
        Hss=6
    }
}
