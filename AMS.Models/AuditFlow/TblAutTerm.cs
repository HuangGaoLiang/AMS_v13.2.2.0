/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 审核中的数据（学期）
     /// </summary>
    public partial class TblAutTerm
     {
          /// <summary>
          /// 主键
          /// </summary>
         public long AutTermId  { get; set; }

          /// <summary>
          /// 审核表Id
          /// </summary>
         public long AuditId  { get; set; }
          /// <summary>
          /// 校区编号
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 年度
          /// </summary>
         public int Year  { get; set; }

          /// <summary>
          /// 学期类型
          /// </summary>
         public long TermTypeId  { get; set; }

          /// <summary>
          /// 学期名称
          /// </summary>
         public string TermName  { get; set; }

          /// <summary>
          /// 开始时间
          /// </summary>
         public DateTime BeginDate  { get; set; }

          /// <summary>
          /// 结束时间
          /// </summary>
         public DateTime EndDate  { get; set; }

          /// <summary>
          /// 60分钟课次
          /// </summary>
         public int Classes60  { get; set; }

        /// <summary>
        /// 90分钟课次
        /// </summary>
        public int Classes90  { get; set; }

        /// <summary>
        /// 180分钟课次
        /// </summary>
        public int Classes180  { get; set; }

        /// <summary>
        /// 单次课学费
        /// </summary>
        public int TuitionFee  { get; set; }

        /// <summary>
        /// 单次课杂费
        /// </summary>
        public int MaterialFee  { get; set; }

        /// <summary>
        /// 源数据ID
        /// </summary>
        public long TermId  { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public int DataStatus  { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime  { get; set; }

     }
}
