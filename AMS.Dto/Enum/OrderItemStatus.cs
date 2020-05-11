using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  报名订单课程枚举
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum OrderItemStatus
    {
        /// <summary>
        /// 已作废
        /// </summary>
        [Description("已作废")]
        Cancel = -1,

        /// <summary>
        /// 已报名
        /// </summary>
        [Description("已报名")]
        Enroll = 1,

        /// <summary>
        /// 已退费
        /// </summary>
        [Description("已退费")]
        LeaveClass = 2,

        /// <summary>
        /// 已休学
        /// </summary>
        [Description("已休学")]
        LeaveSchool = 3,

        /// <summary>
        /// 已转校
        /// </summary>
        [Description("已转校")]
        ChangeSchool = 4
    }
}
