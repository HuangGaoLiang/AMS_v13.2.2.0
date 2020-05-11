/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单
     /// </summary>
   class TblOdrRefundOrderMap : IEntityTypeConfiguration<TblOdrRefundOrder>
   {
        void IEntityTypeConfiguration<TblOdrRefundOrder>.Configure(EntityTypeBuilder<TblOdrRefundOrder> entity)
        {
           // Table
           entity.ToTable("TblOdrRefundOrder");

           // Primary Key
           entity.HasKey(e => e.RefundOrderId);

           // Properties
           entity.Property(e => e.RefundOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderNo)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TotalDeductAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Amount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CancelUserId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CancelDate)
                 .IsUnicode(false) ;

           entity.Property(e => e.CancelRemark)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreatorId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreatorName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
           entity.Property(t => t.OrderType).HasColumnName("OrderType");
           entity.Property(t => t.TotalDeductAmount).HasColumnName("TotalDeductAmount");
           entity.Property(t => t.Amount).HasColumnName("Amount");
           entity.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
           entity.Property(t => t.CancelUserId).HasColumnName("CancelUserId");
           entity.Property(t => t.CancelDate).HasColumnName("CancelDate");
           entity.Property(t => t.CancelRemark).HasColumnName("CancelRemark");
           entity.Property(t => t.CreatorId).HasColumnName("CreatorId");
           entity.Property(t => t.CreatorName).HasColumnName("CreatorName");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
