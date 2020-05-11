/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 用户钱包余额交易流水
     /// </summary>
   class TblCashWalletTradeMap : IEntityTypeConfiguration<TblCashWalletTrade>
   {
        void IEntityTypeConfiguration<TblCashWalletTrade>.Configure(EntityTypeBuilder<TblCashWalletTrade> entity)
        {
           // Table
           entity.ToTable("TblCashWalletTrade");

           // Primary Key
           entity.HasKey(e => e.WalletTradeId);

           // Properties
           entity.Property(e => e.WalletTradeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TradeType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TransAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TransBefBalance)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TransAftBalance)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TransDate)
                 .IsRequired()
                 .IsUnicode(false) ;

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.WalletTradeId).HasColumnName("WalletTradeId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.OrderId).HasColumnName("OrderId");
           entity.Property(t => t.TradeType).HasColumnName("TradeType");
           entity.Property(t => t.TransAmount).HasColumnName("TransAmount");
           entity.Property(t => t.TransBefBalance).HasColumnName("TransBefBalance");
           entity.Property(t => t.TransAftBalance).HasColumnName("TransAftBalance");
           entity.Property(t => t.TransDate).HasColumnName("TransDate");
           entity.Property(t => t.Remark).HasColumnName("Remark");
        }
   }
}
