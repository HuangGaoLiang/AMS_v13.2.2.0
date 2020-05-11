/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 
     /// </summary>
   class TblDatCostMiddleMap : IEntityTypeConfiguration<TblDatCostMiddle>
   {
        void IEntityTypeConfiguration<TblDatCostMiddle>.Configure(EntityTypeBuilder<TblDatCostMiddle> entity)
        {
           // Table
           entity.ToTable("TblDatCostMiddle");

           // Primary Key
           entity.HasKey(e => e.TypeCode);

           // Properties
           entity.Property(e => e.TypeCode)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .ValueGeneratedNever()
                 .IsUnicode(false) ;

           entity.Property(e => e.CostId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.TypeCode).HasColumnName("TypeCode");
           entity.Property(t => t.CostId).HasColumnName("CostId");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
