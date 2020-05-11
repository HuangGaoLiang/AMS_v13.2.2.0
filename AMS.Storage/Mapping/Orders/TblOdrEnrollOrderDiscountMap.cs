/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 报名订单优惠
    /// </summary>
    class TblOdrEnrollOrderDiscountMap : IEntityTypeConfiguration<TblOdrEnrollOrderDiscount>
    {
        void IEntityTypeConfiguration<TblOdrEnrollOrderDiscount>.Configure(EntityTypeBuilder<TblOdrEnrollOrderDiscount> entity)
        {
            // Table
            entity.ToTable("TblOdrEnrollOrderDiscount");

            // Primary Key
            entity.HasKey(e => e.EnrollOrderDiscountId);

            // Properties
            entity.Property(e => e.EnrollOrderDiscountId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.EnrollOrderId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CouponId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CouponAmount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CouponType)
                .IsRequired()
                .IsUnicode(false);

            // Column
            entity.Property(t => t.EnrollOrderDiscountId).HasColumnName("EnrollOrderDiscountId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.EnrollOrderId).HasColumnName("EnrollOrderId");
            entity.Property(t => t.CouponId).HasColumnName("CouponId");
            entity.Property(t => t.CouponAmount).HasColumnName("CouponAmount");
            entity.Property(t => t.CouponType).HasColumnName("CouponType");
        }
    }
}
