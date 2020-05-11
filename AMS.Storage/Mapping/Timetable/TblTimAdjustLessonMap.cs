/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 调整课次业务表
    /// </summary>
    class TblTimAdjustLessonMap : IEntityTypeConfiguration<TblTimAdjustLesson>
    {
        void IEntityTypeConfiguration<TblTimAdjustLesson>.Configure(EntityTypeBuilder<TblTimAdjustLesson> entity)
        {
            // Table
            entity.ToTable("TblTimAdjustLesson");

            // Primary Key
            entity.HasKey(e => e.AdjustLessonId);

            // Properties
            entity.Property(e => e.AdjustLessonId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.BatchNo)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.FromLessonId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.FromTeacherId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.ToTeacherId)
                  .IsRequired()
                  .HasMaxLength(32)
                  .IsUnicode(false);

            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassRoomId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolTimeId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassDate)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ClassBeginTime)
                  .IsRequired()
                  .HasMaxLength(20)
                  .IsUnicode(false);

            entity.Property(e => e.ClassEndTime)
                  .IsRequired()
                  .HasMaxLength(20)
                  .IsUnicode(false);

            entity.Property(e => e.BusinessType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.Status)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false);

            // Column
            entity.Property(t => t.AdjustLessonId).HasColumnName("AdjustLessonId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.BatchNo).HasColumnName("BatchNo");
            entity.Property(t => t.FromLessonId).HasColumnName("FromLessonId");
            entity.Property(t => t.FromTeacherId).HasColumnName("FromTeacherId");
            entity.Property(t => t.ToTeacherId).HasColumnName("ToTeacherId");
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.ClassRoomId).HasColumnName("ClassRoomId");
            entity.Property(t => t.ClassId).HasColumnName("ClassId");
            entity.Property(t => t.SchoolTimeId).HasColumnName("SchoolTimeId");
            entity.Property(t => t.ClassDate).HasColumnName("ClassDate");
            entity.Property(t => t.ClassBeginTime).HasColumnName("ClassBeginTime");
            entity.Property(t => t.ClassEndTime).HasColumnName("ClassEndTime");
            entity.Property(t => t.BusinessType).HasColumnName("BusinessType");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.Status).HasColumnName("Status");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
