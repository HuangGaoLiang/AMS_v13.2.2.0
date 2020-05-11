using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 余额退费请求实体类
    /// </summary>
    public class BalanceRefundListSearchRequest : Page
    {
        /// <summary>
        /// 校区id
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 退费开始日期
        /// </summary>
        public DateTime? RefundBeginDate { get; set; }

        /// <summary>
        /// 退费结束日期
        /// </summary>
        public DateTime? RefundEndDate { get; set; }

        /// <summary>
        /// 姓名或手机号
        /// </summary>
        public string Keyword { get; set; }
    }
}
