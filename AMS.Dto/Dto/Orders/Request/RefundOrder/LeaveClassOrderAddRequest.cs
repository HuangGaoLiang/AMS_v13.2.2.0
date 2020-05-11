using AMS.Dto.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：退班订单添加数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderAddRequest : IRefundOrderTransactRequest
    {
        /// <summary>
        /// 停课开始日期
        /// </summary>
        [Required]
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 退款类型
        /// </summary>
        public RefundType RefundType { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 上传申请表附件
        /// </summary>
        public List<AttchmentAddRequest> AttachmentUrlList { get; set; } = new List<AttchmentAddRequest>();

        /// <summary>
        /// 退班的课程
        /// </summary>
        public List<long> RefundCourseLesson { get; set; } = new List<long>();

        /// <summary>
        /// 票据情况
        /// </summary>
        public ReceiptType ReceiptType { get; set; }

        /// <summary>
        /// 扣款合计
        /// </summary>
        public List<CostRequest> CostList { get; set; } = new List<CostRequest>();

        /// <summary>
        /// 发卡银行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string BankUserName { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [JsonIgnore]
        public string CreatorId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [JsonIgnore]
        public string CreatorName { get; set; }
    }

    /// <summary>
    /// 扣费信息
    /// </summary>
    public class CostRequest
    {
        /// <summary>
        /// 费用Id
        /// </summary>
        public long CostId { get; set; }

        /// <summary>
        /// 扣款名称
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        /// <summary>
        /// 扣费金额
        /// </summary>
        public decimal CostAmount { get; set; }
    }
}
