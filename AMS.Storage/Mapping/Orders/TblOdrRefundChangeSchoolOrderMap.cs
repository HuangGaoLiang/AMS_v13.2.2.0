/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 退费订单-转校转出
    /// </summary>
    class TblOdrRefundChangeSchoolOrderMap : IEntityTypeConfiguration<TblOdrRefundChangeSchoolOrder>
    {
        void IEntityTypeConfiguration<TblOdrRefundChangeSchoolOrder>.Configure(EntityTypeBuilder<TblOdrRefundChangeSchoolOrder> entity)
        {
            // Table
            entity.ToTable("TblOdrRefundChangeSchoolOrder");

            // Primary Key
            entity.HasKey(e => e.RefundOrderId);

            // Properties
            entity.Property(e => e.RefundOrderId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.InSchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.OutSchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(400)
                  .IsUnicode(false);

            entity.Property(e => e.OutDate)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ReceiptStatus)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TransFromBalance)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalRefundLessonCount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TotalUseLessonCount)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.InSchoolId).HasColumnName("InSchoolId");
            entity.Property(t => t.OutSchoolId).HasColumnName("OutSchoolId");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.OutDate).HasColumnName("OutDate");
            entity.Property(t => t.ReceiptStatus).HasColumnName("ReceiptStatus");
            entity.Property(t => t.TransFromBalance).HasColumnName("TransFromBalance");
            entity.Property(t => t.TotalRefundLessonCount).HasColumnName("TotalRefundLessonCount");
            entity.Property(t => t.TotalUseLessonCount).HasColumnName("TotalUseLessonCount");
        }
    }
}
