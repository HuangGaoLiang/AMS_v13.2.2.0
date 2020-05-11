using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单已交接查询条件
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderHandoverListSearchRequest : Page
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 交接开始日期
        /// </summary>
        public DateTime? HandoverBeginDate { get; set; }

        /// <summary>
        /// 交接结束日期
        /// </summary>
        public DateTime? HandoverEndDate { get; set; }
    }
}
