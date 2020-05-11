/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 报名订单
    /// </summary>
    class TblOdrEnrollOrderMap : IEntityTypeConfiguration<TblOdrEnrollOrder>
    {
        void IEntityTypeConfiguration<TblOdrEnrollOrder>.Configure(EntityTypeBuilder<TblOdrEnrollOrder> entity)
        {
            // Table
            entity.ToTable("TblOdrEnrollOrder");

            // Primary Key
            entity.HasKey(e => e.EnrollOrderId);

            // Properties
            entity.Property(e => e.EnrollOrderId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.OrderNo)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.OrderNewType)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.TotalClassTimes)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalTuitionFee)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalMaterialFee)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalDiscountFee)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.PayType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalTradeAmount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.PayAmount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.UseBalance)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CreateId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.CreateName)
                  .IsRequired()
                  .HasMaxLength(50)
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
            entity.Property(t => t.EnrollOrderId).HasColumnName("EnrollOrderId");
            entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
            entity.Property(t => t.OrderNewType).HasColumnName("OrderNewType");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.TotalClassTimes).HasColumnName("TotalClassTimes");
            entity.Property(t => t.TotalTuitionFee).HasColumnName("TotalTuitionFee");
            entity.Property(t => t.TotalMaterialFee).HasColumnName("TotalMaterialFee");
            entity.Property(t => t.TotalDiscountFee).HasColumnName("TotalDiscountFee");
            entity.Property(t => t.PayType).HasColumnName("PayType");
            entity.Property(t => t.TotalTradeAmount).HasColumnName("TotalTradeAmount");
            entity.Property(t => t.PayAmount).HasColumnName("PayAmount");
            entity.Property(t => t.UseBalance).HasColumnName("UseBalance");
            entity.Property(t => t.CreateId).HasColumnName("CreateId");
            entity.Property(t => t.CreateName).HasColumnName("CreateName");
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
