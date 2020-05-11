using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 名单来源
    /// </summary>
    public enum ListSource
    {
        /// <summary>
        /// 自然到访
        /// </summary>
        [Description("自然到访")]
        NaturalVisit = 1,
        /// <summary>
        /// 线下市场活动
        /// </summary>
        [Description("自然到访")]
        OfflineMarketVctivities = 2,
        /// <summary>
        /// 线上活动
        /// </summary>
        [Description("线上活动")]
        OnlineActivity = 3,
        /// <summary>
        /// 地推
        /// </summary>
        [Description("地推")]
        EarthPush = 4,
        /// <summary>
        /// 异业合作
        /// </summary>
        [Description("异业合作")]
        CooperationInDifferentIndustries = 5,
        /// <summary>
        /// 400电话
        /// </summary>
        [Description("400电话")]
        Cell = 6,
        /// <summary>
        /// 转介绍
        /// </summary>
        [Description("转介绍")]
        Transfer = 7,
    }
}
