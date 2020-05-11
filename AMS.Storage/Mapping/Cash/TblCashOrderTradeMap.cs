/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// TblCashOrderTrade
     /// </summary>
   class TblCashOrderTradeMap : IEntityTypeConfiguration<TblCashOrderTrade>
   {
        void IEntityTypeConfiguration<TblCashOrderTrade>.Configure(EntityTypeBuilder<TblCashOrderTrade> entity)
        {
           // Table
           entity.ToTable("TblCashOrderTrade");

           // Primary Key
           entity.HasKey(e => e.OrderTradeId);

           // Properties
           entity.Property(e => e.OrderTradeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

            entity.Property(e => e.OrderNo)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.TradeType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PayType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TradeAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TradeBalanceAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TotalDiscount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Buyer)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Seller)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TradeStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.OrderTradeId).HasColumnName("OrderTradeId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.OrderId).HasColumnName("OrderId");
            entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
            entity.Property(t => t.TradeType).HasColumnName("TradeType");
           entity.Property(t => t.PayType).HasColumnName("PayType");
           entity.Property(t => t.TradeAmount).HasColumnName("TradeAmount");
           entity.Property(t => t.TradeBalanceAmount).HasColumnName("TradeBalanceAmount");
           entity.Property(t => t.TotalDiscount).HasColumnName("TotalDiscount");
           entity.Property(t => t.Buyer).HasColumnName("Buyer");
           entity.Property(t => t.Seller).HasColumnName("Seller");
           entity.Property(t => t.TradeStatus).HasColumnName("TradeStatus");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
