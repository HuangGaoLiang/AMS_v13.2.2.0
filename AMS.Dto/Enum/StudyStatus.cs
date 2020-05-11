using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  学生状态
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum StudyStatus
    {
        /// <summary>
        /// 在读
        /// </summary>
        [Description("在读")]
        Reading = 1,

        /// <summary>
        /// 休学
        /// </summary>
        [Description("休学")]
        Suspension = 2,

        /// <summary>
        /// 流失
        /// </summary>
        [Description("流失")]
        Loss = 3,
    }
}
