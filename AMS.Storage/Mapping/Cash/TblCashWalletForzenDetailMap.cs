/*此代码由生成工具字段生成，生成时间14/11/2018 20:16:09 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 用户钱包余额冻结明细
     /// </summary>
   class TblCashWalletForzenDetailMap : IEntityTypeConfiguration<TblCashWalletForzenDetail>
   {
        void IEntityTypeConfiguration<TblCashWalletForzenDetail>.Configure(EntityTypeBuilder<TblCashWalletForzenDetail> entity)
        {
           // Table
           entity.ToTable("TblCashWalletForzenDetail");

           // Primary Key
           entity.HasKey(e => e.WalletForzenDetailId);

           // Properties
           entity.Property(e => e.WalletForzenDetailId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Amount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Status)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.WalletForzenDetailId).HasColumnName("WalletForzenDetailId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.BusinessType).HasColumnName("BusinessType");
           entity.Property(t => t.BusinessId).HasColumnName("BusinessId");
           entity.Property(t => t.Amount).HasColumnName("Amount");
           entity.Property(t => t.Status).HasColumnName("Status");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
