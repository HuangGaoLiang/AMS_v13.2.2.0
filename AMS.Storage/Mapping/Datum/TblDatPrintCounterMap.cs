/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 打印调用记录
     /// </summary>
   class TblDatPrintCounterMap : IEntityTypeConfiguration<TblDatPrintCounter>
   {
        void IEntityTypeConfiguration<TblDatPrintCounter>.Configure(EntityTypeBuilder<TblDatPrintCounter> entity)
        {
           // Table
           entity.ToTable("TblDatPrintCounter");

           // Primary Key
           entity.HasKey(e => e.PrintCounterId);

           // Properties
           entity.Property(e => e.PrintCounterId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Year)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PrintBillType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Counts)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.PrintCounterId).HasColumnName("PrintCounterId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.Year).HasColumnName("Year");
           entity.Property(t => t.PrintBillType).HasColumnName("PrintBillType");
           entity.Property(t => t.Counts).HasColumnName("Counts");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
