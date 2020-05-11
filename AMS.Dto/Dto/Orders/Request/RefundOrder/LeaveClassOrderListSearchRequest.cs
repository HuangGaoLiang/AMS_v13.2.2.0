using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退班列表查询条件
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderListSearchRequest: Page
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 票据状态
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }
        /// <summary>
        /// 退款开始时间
        /// </summary>
        public DateTime? RefundBeginDate { get; set; }
        /// <summary>
        /// 退款结束时间
        /// </summary>
        public DateTime? RefundEndDate { get; set; }
        /// <summary>
        /// 关键字（学生姓名、手机号）
        /// </summary>
        public string KeyWord { get; set; }

    }
}
