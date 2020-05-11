using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 描    述：报名学习计划学期
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public partial class TblOdrStudyPlanTerm
    {
        /// <summary>
        /// 主键(报名学习计划学期)
        /// </summary>
        public long StudyPlanTermId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 所属学期类型ID
        /// </summary>
        public long TermTypeId { get; set; }
        /// <summary>
        /// 所属学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 60分钟的课次
        /// </summary>
        public int Classes60 { get; set; }
        /// <summary>
        /// 90分钟的课次
        /// </summary>
        public int Classes90 { get; set; }
        /// <summary>
        /// 180分钟的课次
        /// </summary>
        public int Classes180 { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
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
    }
}
