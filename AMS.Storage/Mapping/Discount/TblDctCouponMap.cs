/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    class TblDctCouponMap : IEntityTypeConfiguration<TblDctCoupon>
    {
        void IEntityTypeConfiguration<TblDctCoupon>.Configure(EntityTypeBuilder<TblDctCoupon> entity)
        {
            // Table
            entity.ToTable("TblDctCoupon");

            // Primary Key
            entity.HasKey(e => e.CouponId);

            // Properties
            entity.Property(e => e.CouponId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.CouponNo)
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.CouponType)
                  .IsUnicode(false);

            entity.Property(e => e.Amount)
                  .IsUnicode(false);

            entity.Property(e => e.Status)
                  .IsUnicode(false);

            entity.Property(e => e.IsFreeAll).IsUnicode(false);

            entity.Property(e => e.ExpireTime)
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsUnicode(false);

            entity.Property(e => e.UseTime)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsUnicode(false);

            entity.Property(e => e.FromId)
                  .IsUnicode(false);
            entity.Property(e => e.EnrollOrderId)
                .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.CreatorId)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Column
            entity.Property(t => t.CouponId).HasColumnName("CouponId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.CouponNo).HasColumnName("CouponNo");
            entity.Property(t => t.CouponType).HasColumnName("CouponType");
            entity.Property(t => t.Amount).HasColumnName("Amount");
            entity.Property(t => t.Status).HasColumnName("Status");
            entity.Property(t => t.IsFreeAll).HasColumnName("IsFreeAll");
            entity.Property(t => t.ExpireTime).HasColumnName("ExpireTime");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.UseTime).HasColumnName("UseTime");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
            entity.Property(t => t.FromId).HasColumnName("FromId");
            entity.Property(t => t.EnrollOrderId).HasColumnName("EnrollOrderId");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreatorId).HasColumnName("CreatorId");
        }
    }
}
