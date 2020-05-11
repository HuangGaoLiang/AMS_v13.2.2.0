/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 订金表
    /// </summary>
    class TblOdrDepositOrderMap : IEntityTypeConfiguration<TblOdrDepositOrder>
    {
        void IEntityTypeConfiguration<TblOdrDepositOrder>.Configure(EntityTypeBuilder<TblOdrDepositOrder> entity)
        {
            // Table
            entity.ToTable("TblOdrDepositOrder");

            // Primary Key
            entity.HasKey(e => e.DepositOrderId);

            // Properties
            entity.Property(e => e.DepositOrderId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.OrderNo)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.PayDate)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.UsesType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Amount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.PayeeId)
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.Payee)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.PayType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(400)
                  .IsUnicode(false);

            entity.Property(e => e.OrderStatus)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CancelUserId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.CancelUserName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CancelDate)
                 .IsUnicode(false);

            entity.Property(e => e.CancelRemark)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.DepositOrderId).HasColumnName("DepositOrderId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.PayDate).HasColumnName("PayDate");
            entity.Property(t => t.UsesType).HasColumnName("UsesType");
            entity.Property(t => t.Amount).HasColumnName("Amount");
            entity.Property(t => t.PayeeId).HasColumnName("PayeeId");
            entity.Property(t => t.Payee).HasColumnName("Payee");
            entity.Property(t => t.PayType).HasColumnName("PayType");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            entity.Property(t => t.CancelUserId).HasColumnName("CancelUserId");
            entity.Property(t => t.CancelUserName).HasColumnName("CancelUserName");
            entity.Property(t => t.CancelDate).HasColumnName("CancelDate");
            entity.Property(t => t.CancelRemark).HasColumnName("CancelRemark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
