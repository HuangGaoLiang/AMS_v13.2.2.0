using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    ///转校订单添加数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderAddRequest : IRefundOrderTransactRequest
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
        /// 转校信息
        /// </summary>
        public TransferInformationRequest TransferInformation { get; set; }

        /// <summary>
        /// 费用明细
        /// </summary>
        public FeeDetailRequest FeeDetail { get; set; }

        /// <summary>
        /// 转账信息
        /// </summary>
        public TransferAccountsRequest TransferAccounts { get; set; }
    }

    /// <summary>
    /// 转校信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TransferInformationRequest
    {
        /// <summary>
        /// 转入校区Id
        /// </summary>
        public string TransferToSchoolId { get; set; }

        /// <summary>
        /// 转出日期
        /// </summary>
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// 转校原因
        /// </summary>
        public string ReasonForTransfer { get; set; }

        /// <summary>
        /// 上传申请表附件
        /// </summary>
        public List<AttchmentAddRequest> Attachments { get; set; }
    }

    /// <summary>
    /// 费用明细
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class FeeDetailRequest
    {
        /// <summary>
        /// 余额支付
        /// </summary>
        public decimal PayBalance { get; set; }

        /// <summary>
        /// 报名订单课程ID
        /// </summary>
        public List<long> EnrollOrderItemId { get; set; }

        /// <summary>
        /// 票据情况 1未开发票2发票齐全3发票遗失
        /// </summary>
        public int ReceiptStatus { get; set; }

        /// <summary>
        /// 扣费
        /// </summary>
        public List<CostRequest> Cost { get; set; }
    }

    /// <summary>
    /// 账户信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class TransferAccountsRequest
    {
        /// <summary>
        /// 发卡银行
        /// </summary>
        public string IssuingBank { get; set; }

        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNumber { get; set; }
    }
}
