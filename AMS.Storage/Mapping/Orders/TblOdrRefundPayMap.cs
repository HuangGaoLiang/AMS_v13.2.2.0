/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单-支付信息
     /// </summary>
   class TblOdrRefundPayMap : IEntityTypeConfiguration<TblOdrRefundPay>
   {
        void IEntityTypeConfiguration<TblOdrRefundPay>.Configure(EntityTypeBuilder<TblOdrRefundPay> entity)
        {
           // Table
           entity.ToTable("TblOdrRefundPay");

           // Primary Key
           entity.HasKey(e => e.RefundPayId);

           // Properties
           entity.Property(e => e.RefundPayId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.RefundOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.RefundType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BankName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BankCardNo)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BankUserName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundPayId).HasColumnName("RefundPayId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
           entity.Property(t => t.RefundType).HasColumnName("RefundType");
           entity.Property(t => t.BankName).HasColumnName("BankName");
           entity.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
           entity.Property(t => t.BankUserName).HasColumnName("BankUserName");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
