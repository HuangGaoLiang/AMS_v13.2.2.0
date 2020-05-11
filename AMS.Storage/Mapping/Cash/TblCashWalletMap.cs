/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 用户钱包
     /// </summary>
   class TblCashWalletMap : IEntityTypeConfiguration<TblCashWallet>
   {
        void IEntityTypeConfiguration<TblCashWallet>.Configure(EntityTypeBuilder<TblCashWallet> entity)
        {
           // Table
           entity.ToTable("TblCashWallet");

           // Primary Key
           entity.HasKey(e => e.WalletId);

           // Properties
           entity.Property(e => e.WalletId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Balance)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.FrozenAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.WalletId).HasColumnName("WalletId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.Balance).HasColumnName("Balance");
           entity.Property(t => t.FrozenAmount).HasColumnName("FrozenAmount");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
