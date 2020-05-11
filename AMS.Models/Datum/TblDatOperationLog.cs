using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 业务操作日志
    /// </summary>
    public partial class TblDatOperationLog
    {
        /// <summary>
        /// 主健->业务操作日志
        /// </summary>
        public long OperationLogId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public int BusinessType { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }
        /// <summary>
        /// 操作状态 -2拒绝 -1撤销0提交1确认10接收
        /// </summary>
        public int FlowStatus { get; set; }
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
