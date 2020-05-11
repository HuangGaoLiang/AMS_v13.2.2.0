/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 业务操作日志
     /// </summary>
   class TblDatOperationLogMap : IEntityTypeConfiguration<TblDatOperationLog>
   {
        void IEntityTypeConfiguration<TblDatOperationLog>.Configure(EntityTypeBuilder<TblDatOperationLog> entity)
        {
           // Table
           entity.ToTable("TblDatOperationLog");

           // Primary Key
           entity.HasKey(e => e.OperationLogId);

           // Properties
           entity.Property(e => e.OperationLogId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.FlowStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OperatorId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OperatorName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.OperationLogId).HasColumnName("OperationLogId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.BusinessType).HasColumnName("BusinessType");
           entity.Property(t => t.BusinessId).HasColumnName("BusinessId");
           entity.Property(t => t.FlowStatus).HasColumnName("FlowStatus");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.OperatorId).HasColumnName("OperatorId");
           entity.Property(t => t.OperatorName).HasColumnName("OperatorName");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
