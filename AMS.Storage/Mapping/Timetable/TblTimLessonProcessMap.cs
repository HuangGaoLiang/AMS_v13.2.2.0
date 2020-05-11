/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 课次变更记录表
     /// </summary>
   class TblTimLessonProcessMap : IEntityTypeConfiguration<TblTimLessonProcess>
   {
        void IEntityTypeConfiguration<TblTimLessonProcess>.Configure(EntityTypeBuilder<TblTimLessonProcess> entity)
        {
            // Table
            entity.ToTable("TblTimLessonProcess");

            // Primary Key
            entity.HasKey(e => e.LessonProcessId);

            // Properties
            entity.Property(e => e.LessonProcessId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.RootRawLessonId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.RawLessonId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.LessonId)
                 .IsRequired()
                 .IsUnicode(false);

            entity.Property(e => e.ProcessStatus)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.BusinessType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.BusinessId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ProcessTime)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.LessonProcessId).HasColumnName("LessonProcessId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.RootRawLessonId).HasColumnName("RootRawLessonId");
            entity.Property(t => t.RawLessonId).HasColumnName("RawLessonId");
            entity.Property(t => t.LessonId).HasColumnName("LessonId");
            entity.Property(t => t.ProcessStatus).HasColumnName("ProcessStatus");
            entity.Property(t => t.BusinessType).HasColumnName("BusinessType");
            entity.Property(t => t.BusinessId).HasColumnName("BusinessId");
            entity.Property(t => t.ProcessTime).HasColumnName("ProcessTime");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
