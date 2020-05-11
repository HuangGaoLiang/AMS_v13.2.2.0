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
    class TblDatFeedbackMap : IEntityTypeConfiguration<TblDatFeedback>
    {
        void IEntityTypeConfiguration<TblDatFeedback>.Configure(EntityTypeBuilder<TblDatFeedback> entity)
        {
            // Table
            entity.ToTable("TblDatFeedback");

            // Primary Key
            entity.HasKey(e => e.FeedbackId);

            // Properties
            entity.Property(e => e.FeedbackId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CompanyId)
                 .IsRequired()
                 .HasMaxLength(32)
                 .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.SchoolName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
               .IsRequired()
               .IsUnicode(false);

            entity.Property(e => e.ParentUserCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LinkMobile)
              .IsRequired()
              .HasMaxLength(50)
              .IsUnicode(false);

            entity.Property(e => e.CreatorName)
              .IsRequired()
              .HasMaxLength(50)
              .IsUnicode(false);

            entity.Property(e => e.Content)
              .IsRequired()
              .HasMaxLength(1000)
              .IsUnicode(false);

            entity.Property(e => e.ProcessStatus)
              .IsRequired()
              .IsUnicode(false);

            entity.Property(e => e.ProcessRemark)
             .IsRequired()
             .HasMaxLength(1000)
             .IsUnicode(false);

            entity.Property(e => e.ProcessTime)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.ProcessUserId)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.ProcessUserName)
             .IsRequired()
             .HasMaxLength(50)
             .IsUnicode(false);


            entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false);

            // Column
            entity.Property(t => t.FeedbackId).HasColumnName("FeedbackId");
            entity.Property(t => t.CompanyId).HasColumnName("CompanyId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.SchoolName).HasColumnName("SchoolName");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.ParentUserCode).HasColumnName("ParentUserCode");
            entity.Property(t => t.LinkMobile).HasColumnName("LinkMobile");
            entity.Property(t => t.CreatorName).HasColumnName("CreatorName");
            entity.Property(t => t.Content).HasColumnName("Content");
            entity.Property(t => t.ProcessStatus).HasColumnName("ProcessStatus");
            entity.Property(t => t.ProcessRemark).HasColumnName("ProcessRemark");
            entity.Property(t => t.ProcessTime).HasColumnName("ProcessTime");
            entity.Property(t => t.ProcessUserId).HasColumnName("ProcessUserId");
            entity.Property(t => t.ProcessUserName).HasColumnName("ProcessUserName");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
