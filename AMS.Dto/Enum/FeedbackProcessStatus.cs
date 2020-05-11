using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  投诉与建议处理状态枚举
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-03-07</para>
    /// </summary>
    public enum FeedbackProcessStatus
    {
        /// <summary>
        ///  待处理
        /// </summary>
        [Description("待处理")]
        ToBeProcessed = 1,

        /// <summary>
        /// 未解决
        /// </summary>
        [Description("未解决")]
        Unsolved = 2,

        /// <summary>
        /// 已解决
        /// </summary>
        [Description("已解决")]
        Solved = 3,
    }
}
