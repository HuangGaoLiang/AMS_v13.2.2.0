using System;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：收款交接信息实体请求类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-19</para>
    /// </summary>
    public class OrderHandoverRequest
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        [JsonIgnore]
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员Id
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 招生专员名称
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 收现钞
        /// </summary>
        public decimal DayIncomeAmout { get; set; }

        /// <summary>
        /// 存入银行现钞
        /// </summary>
        public decimal InBankAmount { get; set; }

        /// <summary>
        /// 存单张数
        /// </summary>
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// 招生专员签字
        /// </summary>
        public string HandoverUrl { get; set; }

        /// <summary>
        /// 驻校出纳签字
        /// </summary>
        public string RecipientUrl { get; set; }

        /// <summary>
        /// 差异说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 交接日期
        /// </summary>
        [JsonIgnore]
        public DateTime HandoverDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonIgnore]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonIgnore]
        public string CreatorId { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonIgnore]
        public string CreatorName { get; set; }
    }
}
