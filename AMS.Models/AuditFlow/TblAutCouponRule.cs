/*此代码由生成工具字段生成，生成时间2018/10/27 17:55:53 */
using System;

namespace YMM.Storage.Models
{
     /// <summary>
     /// 
     /// </summary>
    public partial class TblAutCouponRule 
     {
          /// <summary>
          /// 主键
          /// </summary>
         public long AutCouponRuleId  { get; set; }

         /// <summary>
          /// 优惠ID
          /// </summary>
         public long CouponRuleId { get; set; }

          /// <summary>
          /// 所属审核
          /// </summary>
         public long AuditId  { get; set; }

          /// <summary>
          /// 所属校区
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 优惠名称
          /// </summary>
         public string CouponRuleName  { get; set; }

          /// <summary>
          /// 优惠类型
          /// </summary>
         public int CouponType  { get; set; }

          /// <summary>
          /// 满多少
          /// </summary>
         public decimal FullAmount  { get; set; }

          /// <summary>
          /// 优惠额度
          /// </summary>
         public decimal CouponAmount  { get; set; }

          /// <summary>
          /// 名额限制
          /// </summary>
         public int MaxQuota  { get; set; }

        /// <summary>
        /// 优惠起始日期
        /// </summary>
        public DateTime BeginDate  { get; set; }

          /// <summary>
          /// 优惠截止日期
          /// </summary>
         public DateTime EndDate  { get; set; }

          /// <summary>
          /// 优惠原因
          /// </summary>
         public string Remark  { get; set; }

          /// <summary>
          /// 创建时间
          /// </summary>
         public DateTime CreateTime  { get; set; }

     }
}
