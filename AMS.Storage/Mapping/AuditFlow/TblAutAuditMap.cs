/*此代码由生成工具字段生成，生成时间2018/9/10 16:44:45 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 审核表
    /// </summary>
    class TblAutAuditMap : IEntityTypeConfiguration<TblAutAudit>
    {
        void IEntityTypeConfiguration<TblAutAudit>.Configure(EntityTypeBuilder<TblAutAudit> entity)
        {
            // Table
            entity.ToTable("TblAutAudit");

            // Primary Key
            entity.HasKey(e => e.AuditId);

            // Properties
            entity.Property(e => e.AuditId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                .IsRequired().IsUnicode(false);

            entity.Property(e => e.FlowNo)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.BizType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.AuditStatus)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.AuditUserId)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);
            entity.Property(e => e.AuditUserName)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);

            entity.Property(e => e.AuditDate)
                .IsRequired()
                .IsUnicode();

            entity.Property(e => e.CreateUserId).IsRequired().HasMaxLength(50).IsUnicode(false);

            entity.Property(e => e.CreateUserName).IsRequired().HasMaxLength(50).IsUnicode(false);

            entity.Property(e => e.DataExt)
                 .IsRequired()
                 .HasMaxLength(4000)
                 .IsUnicode(false);

            entity.Property(e => e.DataExtVersion)
                  .IsRequired()
                  .HasMaxLength(10)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.UpdateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.AuditId).HasColumnName("AuditId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.FlowNo).HasColumnName("FlowNo");
            entity.Property(t => t.BizType).HasColumnName("BizType");
            entity.Property(t => t.AuditStatus).HasColumnName("AuditStatus");
            entity.Property(t => t.AuditUserId).HasColumnName("AuditUserId");
            entity.Property(t => t.AuditUserName).HasColumnName("AuditUserName");
            entity.Property(t => t.AuditDate).HasColumnName("AuditDate");
            entity.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            entity.Property(t => t.CreateUserName).HasColumnName("CreateUserName");
            entity.Property(t => t.DataExt).HasColumnName("DataExt");
            entity.Property(t => t.DataExtVersion).HasColumnName("DataExtVersion");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
            entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
