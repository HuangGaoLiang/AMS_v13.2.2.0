/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 转班表
    /// </summary>
    class TblTimChangeClassMap : IEntityTypeConfiguration<TblTimChangeClass>
    {
        void IEntityTypeConfiguration<TblTimChangeClass>.Configure(EntityTypeBuilder<TblTimChangeClass> entity)
        {
            // Table
            entity.ToTable("TblTimChangeClass");

            // Primary Key
            entity.HasKey(e => e.ChangeClassId);

            // Properties
            entity.Property(e => e.ChangeClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.EnrollOrderItemId).IsRequired().IsUnicode(false);

            entity.Property(e => e.OutClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.InClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.InDate)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.OutDate)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassTimes)
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
            entity.Property(t => t.ChangeClassId).HasColumnName("ChangeClassId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.OutClassId).HasColumnName("OutClassId");
            entity.Property(t => t.InClassId).HasColumnName("InClassId");
            entity.Property(t => t.InDate).HasColumnName("InDate");
            entity.Property(t => t.OutDate).HasColumnName("OutDate");
            entity.Property(t => t.ClassTimes).HasColumnName("ClassTimes");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
