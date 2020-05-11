using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：订单交接明细请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderHandoverDetailRequest : Page
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 订单交接Id
        /// </summary>
        public long HandoverId { get; set; }
    }
}
