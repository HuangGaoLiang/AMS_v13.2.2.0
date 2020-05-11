/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 审核表
    /// </summary>
    public partial class TblAutAudit
    {
        /// <summary>
        /// 审核表主键
        /// </summary>
        public long AuditId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public string FlowNo { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BizType { get; set; }
        /// <summary>
        /// 业务扩展字段1
        /// </summary>
        public string ExtField1 { get; set; }
        /// <summary>
        /// 业务扩展字段2
        /// </summary>
        public string ExtField2 { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int AuditStatus { get; set; }
        /// <summary>
        /// 审核人Id
        /// </summary>
        public string AuditUserId { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUserName { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 数据扩展
        /// </summary>
        public string DataExt { get; set; }
        /// <summary>
        /// 数据版本
        /// </summary>
        public string DataExtVersion { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
