using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 学生考勤状态
    /// </summary>
    public enum AttendStatus
    {
        /// <summary>
        /// 未考勤/缺勤
        /// </summary>
        [Description("未考勤")]
        NotClockIn = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        ///// <summary>
        ///// 补签
        ///// </summary>
        //[Description("补签")]
        //Supplement = 2,
        /// <summary>
        /// 请假
        /// </summary>
        [Description("请假")]
        Leave = 2,
    }
}
