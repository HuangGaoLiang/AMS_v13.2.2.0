using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生列表查询条件
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-09</para>
    /// </summary>
    public class LifeClassListSearchRequest : Page
    {
        /// <summary>
        /// 学期Id
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }
    }
}
