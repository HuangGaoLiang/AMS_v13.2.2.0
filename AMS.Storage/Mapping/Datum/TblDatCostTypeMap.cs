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
   class TblDatCostTypeMap : IEntityTypeConfiguration<TblDatCostType>
   {
        void IEntityTypeConfiguration<TblDatCostType>.Configure(EntityTypeBuilder<TblDatCostType> entity)
        {
           // Table
           entity.ToTable("TblDatCostType");

           // Primary Key
           entity.HasKey(e => e.TblDatCostTypeId);

           // Properties
           entity.Property(e => e.TblDatCostTypeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TypeCode)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TypeName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.TblDatCostTypeId).HasColumnName("TblDatCostTypeId");
           entity.Property(t => t.TypeCode).HasColumnName("TypeCode");
           entity.Property(t => t.TypeName).HasColumnName("TypeName");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
