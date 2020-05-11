using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  性别
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("未知")]
        No = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 2
    }
}
