using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 证件类型
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        ID = 1,
        /// <summary>
        /// 护照
        /// </summary>
        [Description("护照")]
        Passport = 2,
        /// <summary>
        /// 港澳通行证
        /// </summary>
        [Description("港澳通行证")]
        HongKongAndMacaoPass = 3,
        /// <summary>
        /// 台胞证
        /// </summary>
        [Description("台胞证")]
        CellSyndrome = 4
    }
}
