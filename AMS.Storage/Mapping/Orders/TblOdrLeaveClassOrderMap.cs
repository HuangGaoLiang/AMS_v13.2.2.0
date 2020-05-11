/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单-退班
     /// </summary>
   class TblOdrLeaveClassOrderMap : IEntityTypeConfiguration<TblOdrLeaveClassOrder>
   {
        void IEntityTypeConfiguration<TblOdrLeaveClassOrder>.Configure(EntityTypeBuilder<TblOdrLeaveClassOrder> entity)
        {
           // Table
           entity.ToTable("TblOdrLeaveClassOrder");

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

           entity.Property(e => e.StopClassDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Reason)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ReceiptStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StopClassDate).HasColumnName("StopClassDate");
           entity.Property(t => t.Reason).HasColumnName("Reason");
           entity.Property(t => t.ReceiptStatus).HasColumnName("ReceiptStatus");
        }
   }
}
