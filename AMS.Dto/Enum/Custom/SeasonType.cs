using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：季节类型
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-02-21</para>
    /// </summary>
    public enum SeasonType
    {
        /// <summary>
        /// 春季
        /// </summary>
        [Description("春季")]
        Spring,
        /// <summary>
        /// 夏季
        /// </summary>
        [Description("夏季")]
        Summer,
        /// <summary>
        /// 秋季
        /// </summary>
        [Description("秋季")]
        Autumn,
        /// <summary>
        /// 冬季
        /// </summary>
        [Description("冬季")]
        Winter
    }
}
