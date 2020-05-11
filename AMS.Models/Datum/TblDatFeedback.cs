using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 投诉与建议
    /// </summary>
    public partial class TblDatFeedback
    {
        /// <summary>
        /// 主键 
        /// </summary>
        public long FeedbackId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 所属校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 家长账号
        /// </summary>
        public string ParentUserCode { get; set; }

        /// <summary>
        /// 联系手机号 
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 提交人名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 反馈内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public int? ProcessStatus { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string ProcessRemark { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessTime { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        public string ProcessUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string ProcessUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
