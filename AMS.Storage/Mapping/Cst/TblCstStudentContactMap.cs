/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Storage.Models.Storage.Mapping
{
    /// <summary>
    /// 学生联系人
    /// </summary>
    class TblCstStudentContactMap : IEntityTypeConfiguration<TblCstStudentContact>
    {
        void IEntityTypeConfiguration<TblCstStudentContact>.Configure(EntityTypeBuilder<TblCstStudentContact> entity)
        {
            // Table
            entity.ToTable("TblCstStudentContact");

            // Primary Key
            entity.HasKey(e => e.StudentContactId);

            // Properties
            entity.Property(e => e.StudentContactId)
                 .IsRequired()
                 .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.GuardianName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.Relationship)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.Mobile)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);

            entity.Property(e => e.Email)
                 .IsRequired()
                 .HasMaxLength(100)
                 .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.StudentContactId).HasColumnName("StudentContactId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.GuardianName).HasColumnName("GuardianName");
            entity.Property(t => t.Relationship).HasColumnName("Relationship");
            entity.Property(t => t.Mobile).HasColumnName("Mobile");
            entity.Property(t => t.Email).HasColumnName("Email");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
