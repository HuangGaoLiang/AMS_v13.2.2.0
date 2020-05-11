/*此代码由生成工具字段生成，生成时间2018/10/27 17:55:53 */
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Storage.Models;

namespace YMM.Storage.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    class TblAutCouponRuleMap : IEntityTypeConfiguration<TblAutCouponRule>
   {
        void IEntityTypeConfiguration<TblAutCouponRule>.Configure(EntityTypeBuilder<TblAutCouponRule> entity)
        {
           // Table
           entity.ToTable("TblAutCouponRule");

           // Primary Key
           entity.HasKey(e => e.AutCouponRuleId);

           // Properties
           entity.Property(e => e.AutCouponRuleId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CouponRuleId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AuditId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CouponRuleName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CouponType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.FullAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CouponAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaxQuota)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BeginDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EndDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(400) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.AutCouponRuleId).HasColumnName("AutCouponRuleId");
           entity.Property(t => t.CouponRuleId).HasColumnName("CouponRuleId");
           entity.Property(t => t.AuditId).HasColumnName("AuditId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.CouponRuleName).HasColumnName("CouponRuleName");
           entity.Property(t => t.CouponType).HasColumnName("CouponType");
           entity.Property(t => t.FullAmount).HasColumnName("FullAmount");
           entity.Property(t => t.CouponAmount).HasColumnName("CouponAmount");
           entity.Property(t => t.MaxQuota).HasColumnName("MaxQuota");
           entity.Property(t => t.BeginDate).HasColumnName("BeginDate");
           entity.Property(t => t.EndDate).HasColumnName("EndDate");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
