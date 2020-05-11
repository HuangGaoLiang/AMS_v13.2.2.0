using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：提交审核数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class TermAuditRequest
    {
        /// <summary>
        /// 下一步审核人
        /// </summary>
        [Required]
        public string AuditUserId { get; set; }
        /// <summary>
        /// 下一步审核人姓名
        /// </summary>
        [Required]
        public string AuditUserName { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [JsonIgnore]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 审核数据详细
        /// </summary>
        public List<TermAuditDetailRequest> TermAuditDetail { get; set; } = new List<TermAuditDetailRequest>();
    }

    /// <summary>
    /// 描述：审核数据详细
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class TermAuditDetailRequest
    {
        /// <summary>
        /// 学期Id，新增学期ID为空
        /// </summary>
        [Required]
        public long TermId { get; set; }
        /// <summary>
        /// 学期类型Id
        /// </summary>
        [Required]
        public long TermTypeId { get; set; }
        /// <summary>
        /// 学期名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TermName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 60分钟课次
        /// </summary>
        [Required]
        public int Classes60 { get; set; }
        /// <summary>
        /// 90分钟课次
        /// </summary>
        [Required]
        public int Classes90 { get; set; }
        /// <summary>
        /// 180分钟课次
        /// </summary>
        [Required]
        public int Classes180 { get; set; }
        /// <summary>
        /// 单次课学费
        /// </summary>
        [Required]
        public int TuitionFee { get; set; }
        /// <summary>
        /// 单次课杂费
        /// </summary>
        [Required]
        public int MaterialFee { get; set; }
    }
}
