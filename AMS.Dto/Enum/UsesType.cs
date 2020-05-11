using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  订金用途
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-09-07</para>
    /// </summary>
    public enum UsesType
    {
        /// <summary>
        /// 预报名
        /// </summary>
        [Description("预报名")]
        SignUp = 1,

        /// <summary>
        /// 游学营定金
        /// </summary>
        [Description("游学营定金")]
        StudyCamp = 2,

        /// <summary>
        /// 写生营
        /// </summary>
        [Description("写生营")]
        SketchingCamp = 3
    }
}
