using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum AuditBusinessType
    {
        /// <summary>
        /// 学期
        /// </summary>
        [Description("学期")]
        Term = 1,
        /// <summary>
        /// 学期排课
        /// </summary>
        [Description("学期排课")]
        TermCourseTimetable=2,
        /// <summary>
        /// 赠与奖学金
        /// </summary>
        [Description("赠与奖学金")]
        ScholarshipGive=3,
    }
}
