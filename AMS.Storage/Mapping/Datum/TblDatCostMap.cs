/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 费用类型表
     /// </summary>
   class TblDatCostMap : IEntityTypeConfiguration<TblDatCost>
   {
        void IEntityTypeConfiguration<TblDatCost>.Configure(EntityTypeBuilder<TblDatCost> entity)
        {
           // Table
           entity.ToTable("TblDatCost");

           // Primary Key
           entity.HasKey(e => e.CostId);

           // Properties
           entity.Property(e => e.CostId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CostName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.IsDisabled)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.CostId).HasColumnName("CostId");
           entity.Property(t => t.CostName).HasColumnName("CostName");
           entity.Property(t => t.IsDisabled).HasColumnName("IsDisabled");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
