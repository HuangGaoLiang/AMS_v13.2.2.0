using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：审核回调方法请求实体
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2018-9-7</para>
    /// </summary>
    public class AuditCallbackRequest
    {
        /// <summary>
        /// 流程实例Id
        /// </summary>
        public string WFInstanceId { get; set; }
        /// <summary>
        /// 业务编码
        /// </summary>
        public string BussinessCode { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        public string ApplyNumber { get; set; }
        /// <summary>
        /// 审批审人Id
        /// </summary>
        public string AuditUserId { get; set; }
        /// <summary>
        /// 审核人名称
        /// </summary>
        public string AuditUserName { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 描述信息(审批结果)
        /// </summary>
        public string Descption { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public AuditStatus Status { get; set; }

        /// <summary>
        /// 退回原因
        /// </summary>
        public string Remark { get; set; }
    }
}
