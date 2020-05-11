using System.Collections.Generic;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 财务确认数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderConfirmRequest
    {
        /// <summary>
        /// 操作人Id
        /// </summary>
        [JsonIgnore]
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [JsonIgnore]
        public string OperatorName { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId { get; set; }

        //只允许修改：票据情况、扣款、发卡银行、账户姓名、银行卡号

        /// <summary>
        /// 票据情况 1未开发票2发票齐全3发票遗失
        /// </summary>
        public int ReceiptStatus { get; set; }

        /// <summary>
        /// 扣费
        /// </summary>
        public List<CostRequest> Cost { get; set; }

        /// <summary>
        /// 转账
        /// </summary>
        public TransferAccountsRequest TransferAccounts { get; set; }
    }
}
