/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 课程级别设置
    /// </summary>
    class TblDatCourseLevelMap : IEntityTypeConfiguration<TblDatCourseLevel>
    {
        void IEntityTypeConfiguration<TblDatCourseLevel>.Configure(EntityTypeBuilder<TblDatCourseLevel> entity)
        {
            // Table
            entity.ToTable("TblDatCourseLevel");

            // Primary Key
            entity.HasKey(e => e.CourseLevelId);

            // Properties
            entity.Property(e => e.CourseLevelId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CompanyId)
                .IsRequired()
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.Property(e => e.LevelCode)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);

            entity.Property(e => e.LevelCnName)
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.LevelEnName)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.IsDisabled)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.UpdateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.CourseLevelId).HasColumnName("CourseLevelId");
            entity.Property(t => t.CompanyId).HasColumnName("CompanyId");
            entity.Property(t => t.LevelCode).HasColumnName("LevelCode");
            entity.Property(t => t.LevelCnName).HasColumnName("LevelCnName");
            entity.Property(t => t.LevelEnName).HasColumnName("LevelEnName");
            entity.Property(t => t.IsDisabled).HasColumnName("IsDisabled");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
            entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
