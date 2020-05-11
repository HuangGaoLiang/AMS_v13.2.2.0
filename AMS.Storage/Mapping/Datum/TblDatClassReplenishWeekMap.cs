/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 补课周信息表
    /// </summary>
    class TblDatClassReplenishWeekMap : IEntityTypeConfiguration<TblDatClassReplenishWeek>
    {
        void IEntityTypeConfiguration<TblDatClassReplenishWeek>.Configure(EntityTypeBuilder<TblDatClassReplenishWeek> entity)
        {
            // Table
            entity.ToTable("TblDatClassReplenishWeek");

            // Primary Key
            entity.HasKey(e => e.ReplenishWeekId);

            // Properties
            entity.Property(e => e.ReplenishWeekId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.TeacherId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(1000)
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.ReplenishWeekId).HasColumnName("ReplenishWeekId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.ClassId).HasColumnName("ClassId");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
