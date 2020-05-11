using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  身份证类型
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum IDType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        IDCard = 1,

        /// <summary>
        /// 护照
        /// </summary>
        [Description("护照")]
        HZ = 2,
        /// <summary>
        /// 港澳通行证
        /// </summary>
        [Description("港澳通行证")]
        GAT = 3,

        /// <summary>
        /// 台胞证
        /// </summary>
        [Description("台胞证")]
        TB = 4
    }
}
