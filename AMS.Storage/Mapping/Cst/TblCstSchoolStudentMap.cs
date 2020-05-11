/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Storage.Models.Storage.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    class TblCstSchoolStudentMap : IEntityTypeConfiguration<TblCstSchoolStudent>
    {
        void IEntityTypeConfiguration<TblCstSchoolStudent>.Configure(EntityTypeBuilder<TblCstSchoolStudent> entity)
        {
            // Table
            entity.ToTable("TblCstSchoolStudent");

            // Primary Key
            entity.HasKey(e => e.SchoolStudentId);
            // Properties
            entity.Property(e => e.SchoolStudentId)
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

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.StudyStatus)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.RemindClassTimes)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.SchoolStudentId).HasColumnName("SchoolStudentId");
            entity.Property(t => t.CompanyId).HasColumnName("CompanyId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.StudyStatus).HasColumnName("StudyStatus");
            entity.Property(t => t.RemindClassTimes).HasColumnName("RemindClassTimes");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
