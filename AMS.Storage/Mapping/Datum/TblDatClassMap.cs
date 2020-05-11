/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 班级
    /// </summary>
    class TblDatClassMap : IEntityTypeConfiguration<TblDatClass>
    {
        void IEntityTypeConfiguration<TblDatClass>.Configure(EntityTypeBuilder<TblDatClass> entity)
        {
            // Table
            entity.ToTable("TblDatClass");

            // Primary Key
            entity.HasKey(e => e.ClassId);

            // Properties
            entity.Property(e => e.ClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.ClassNo)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.TermId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.RoomCourseId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassRoomId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CourseId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CourseLeveId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.TeacherId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.CourseNum)
                  .IsRequired()
                  .IsUnicode(false);


            entity.Property(e => e.StudentsNum)
                 .IsRequired()
                 .IsUnicode(false);

            entity.Property(e => e.ClassType)
             .IsRequired()
             .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.UpdateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.ClassId).HasColumnName("ClassId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.ClassNo).HasColumnName("ClassNo");
            entity.Property(t => t.TermId).HasColumnName("TermId");
            entity.Property(t => t.RoomCourseId).HasColumnName("RoomCourseId");
            entity.Property(t => t.ClassRoomId).HasColumnName("ClassRoomId");
            entity.Property(t => t.CourseId).HasColumnName("CourseId");
            entity.Property(t => t.CourseLeveId).HasColumnName("CourseLeveId");
            entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
            entity.Property(t => t.CourseNum).HasColumnName("CourseNum");
            entity.Property(t => t.StudentsNum).HasColumnName("StudentsNum");
            entity.Property(t => t.ClassType).HasColumnName("ClassType");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
            entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
