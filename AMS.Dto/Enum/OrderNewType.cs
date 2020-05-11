using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  新生/老生
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum OrderNewType
    {
        /// <summary>
        /// 新生
        /// </summary>
        [Description("新生")]
        NewStu = 1,

        /// <summary>
        /// 老生
        /// </summary>
        [Description("老生")]
        OldStu = 2,
    }
}
