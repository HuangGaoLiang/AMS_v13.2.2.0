using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 签到人员类型
    /// </summary>
    public enum AttendUserType
    {
        /// <summary>
        /// 默认值
        /// </summary>
        [Description("默认值")]
        DEFAULT = 0,
        /// <summary>
        /// 老师
        /// </summary>
        [Description("老师")]
        TEACHER = 1,
        /// <summary>
        /// 财务
        /// </summary>
        [Description("财务")]
        FINANCE = 9
    }
}
