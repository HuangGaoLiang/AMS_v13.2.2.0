using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 是否安排补课/调课类型
    /// </summary>
    /// <remarks>
    /// 1已经安排补课2已安排调课3已经安排补课周补课 4补签未确认5补签已确认
    /// </remarks>
    public enum AdjustType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        DEFAULT = 0,

        /// <summary>
        /// 补课
        /// </summary>
        [Description("补课")]
        MAKEUP = 1,

        /// <summary>
        /// 调课
        /// </summary>
        [Description("调课")]
        ADJUST = 2,

        /// <summary>
        /// 补课周补课
        /// </summary>
        [Description("补课周补课")]
        SUPPLEMENTARYWEEK = 3,

        /// <summary>
        /// 补签未确认
        /// </summary>
        [Description("补签未确认")]
        SUPPLEMENTNOTCONFIRMED = 4,

        /// <summary>
        /// 补签已确认
        /// </summary>
        [Description("补签已确认")]
        SUPPLEMENTCONFIRMED = 5,
    }
}
