using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订单明细
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum Week
    {
        /// <summary>
        ///  星期一
        /// </summary>
        [Description("周一")]
        Monday = 1,

        /// <summary>
        ///  星期二
        /// </summary>
        [Description("周二")]
        Tuesday = 2,

        /// <summary>
        /// 星期三
        /// </summary>
        [Description("周三")]
        Wednesday = 3,

        /// <summary>
        /// 星期四
        /// </summary>
        [Description("周四")]
        Thursday = 4,

        /// <summary>
        /// 星期五
        /// </summary>
        [Description("周五")]
        Friday = 5,

        /// <summary>
        /// 星期六
        /// </summary>
        [Description("周六")]
        Saturday = 6,

        /// <summary>
        /// 星期天
        /// </summary>
        [Description("周日")]
        Sunday = 7
    }
}
