using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学期
    /// </summary>
    public partial class TblDatTerm
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long TermId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期类型
        /// </summary>
        public long TermTypeId { get; set; }
        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
