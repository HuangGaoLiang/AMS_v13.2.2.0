/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单-订单课程明细
     /// </summary>
   class TblOdrRefundOrdeEnrollMap : IEntityTypeConfiguration<TblOdrRefundOrdeEnroll>
   {
        void IEntityTypeConfiguration<TblOdrRefundOrdeEnroll>.Configure(EntityTypeBuilder<TblOdrRefundOrdeEnroll> entity)
        {
           // Table
           entity.ToTable("TblOdrRefundOrdeEnroll");

           // Primary Key
           entity.HasKey(e => e.RefundOrderEnrollId);

           // Properties
           entity.Property(e => e.RefundOrderEnrollId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollOrderItemId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.RefundOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.LessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UseLessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.LessonPrice)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Amount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundOrderEnrollId).HasColumnName("RefundOrderEnrollId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.EnrollOrderItemId).HasColumnName("EnrollOrderItemId");
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
           entity.Property(t => t.LessonCount).HasColumnName("LessonCount");
           entity.Property(t => t.UseLessonCount).HasColumnName("UseLessonCount");
           entity.Property(t => t.LessonPrice).HasColumnName("LessonPrice");
           entity.Property(t => t.Amount).HasColumnName("Amount");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
