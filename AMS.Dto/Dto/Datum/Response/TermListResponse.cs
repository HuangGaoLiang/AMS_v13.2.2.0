using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：审核列表数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class TermListResponse
    {
        /// <summary>
        /// 审核数据列表
        /// </summary>
        public List<TermDetailResponse> Data { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus? AuditStatus { get; set; }
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
        /// 审核备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否第一次提交人
        /// </summary>
        public bool IsFirstSubmitUser { get; set; } = true;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 学期类型列表
        /// </summary>
        public List<TermTypeResponse> TermTypeList { get; set; }
    }

    /// <summary>
    /// 描述：审核详细数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-9-15</para>
    /// </summary>
    public class TermDetailResponse
    {
        /// <summary>
        /// 学期Id主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }
        /// <summary>
        /// 学期类型Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermTypeId { get; set; }
        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }
        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 开始时间是否可以编辑
        /// </summary>
        public bool IsBeginDateChange { get; set; }
        /// <summary>
        ///结束时间是否可以编辑
        /// </summary>
        public bool IsEndDateChange { get; set; }
        /// <summary>
        /// 是否当前学期
        /// </summary>
        public bool IsCurrentTerm { get; set; }

        /// <summary>
        /// 60分钟课次
        /// </summary>
        public int Classes60 { get; set; }
        /// <summary>
        /// 90分钟课次
        /// </summary>
        public int Classes90 { get; set; }
        /// <summary>
        /// 180分钟课次
        /// </summary>
        public int Classes180 { get; set; }
        /// <summary>
        /// 单次课学费
        /// </summary>
        public int TuitionFee { get; set; }
        /// <summary>
        /// 单次课杂费
        /// </summary>
        public int MaterialFee { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
    }
}
