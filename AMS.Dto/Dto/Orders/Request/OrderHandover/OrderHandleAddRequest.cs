using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：核对未交接的数据
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderHandleAddRequest
    {
        /// <summary>
        /// 所属校区
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员ID
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal? UseBalanceAmount { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? PayAmount { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [JsonIgnore]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [JsonIgnore]
        public string CreatorName { get; set; }

        /// <summary>
        /// 上传申请表附件
        /// </summary>
        public List<AttchmentAddRequest> AttachmentUrlList { get; set; }
    }
}
