using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学办理的请求数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class LeaveSchoolOrderAddRequest : IRefundOrderTransactRequest
    {
        /// <summary>
        /// 休学日期
        /// </summary>
        [Required]
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }
        /// <summary>
        /// 休学原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 补充说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 上传申请表附件
        /// </summary>
        public List<AttchmentAddRequest> AttachmentUrlList { get; set; } = new List<AttchmentAddRequest>();


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
}
