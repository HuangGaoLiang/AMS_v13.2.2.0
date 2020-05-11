/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 课程表
    /// </summary>
    class TblDatCourseMap : IEntityTypeConfiguration<TblDatCourse>
    {
        void IEntityTypeConfiguration<TblDatCourse>.Configure(EntityTypeBuilder<TblDatCourse> entity)
        {
            // Table
            entity.ToTable("TblDatCourse");

            // Primary Key
            entity.HasKey(e => e.CourseId);

            // Properties
            entity.Property(e => e.CourseId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CompanyId)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CourseCode)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);

            entity.Property(e => e.CourseType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ShortName)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CourseCnName)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CourseEnName)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.ClassCnName)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.ClassEnName)
                  .IsRequired()
                  .HasMaxLength(100)
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
            entity.Property(t => t.CourseId).HasColumnName("CourseId");
            entity.Property(t => t.CompanyId).HasColumnName("CompanyId");
            entity.Property(t => t.CourseCode).HasColumnName("CourseCode");
            entity.Property(t => t.CourseType).HasColumnName("CourseType");
            entity.Property(t => t.ShortName).HasColumnName("ShortName");
            entity.Property(t => t.CourseCnName).HasColumnName("CourseCnName");
            entity.Property(t => t.CourseEnName).HasColumnName("CourseEnName");
            entity.Property(t => t.ClassCnName).HasColumnName("ClassCnName");
            entity.Property(t => t.ClassEnName).HasColumnName("ClassEnName");
            entity.Property(t => t.IsDisabled).HasColumnName("IsDisabled");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
            entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
