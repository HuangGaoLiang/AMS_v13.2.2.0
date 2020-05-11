/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 课次基础信息表
     /// </summary>
   class TblTimLessonMap : IEntityTypeConfiguration<TblTimLesson>
   {
        void IEntityTypeConfiguration<TblTimLesson>.Configure(EntityTypeBuilder<TblTimLesson> entity)
        {
           // Table
           entity.ToTable("TblTimLesson");

           // Primary Key
           entity.HasKey(e => e.LessonId);

           // Properties
           entity.Property(e => e.LessonId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollOrderItemId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassBeginTime)
                 .IsRequired()
                 .HasMaxLength(20) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassEndTime)
                 .IsRequired()
                 .HasMaxLength(20) 
                 .IsUnicode(false) ;


            entity.Property(e => e.ClassBeginDate)
                  .IsUnicode(false);

            entity.Property(e => e.ClassEndDate)
                  .IsUnicode(false);

            entity.Property(e => e.TermId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseLevelId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassRoomId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TeacherId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.LessonType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.LessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Status)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.LessonId).HasColumnName("LessonId");
           entity.Property(t => t.EnrollOrderItemId).HasColumnName("EnrollOrderItemId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.ClassDate).HasColumnName("ClassDate");
           entity.Property(t => t.ClassBeginTime).HasColumnName("ClassBeginTime");
           entity.Property(t => t.ClassEndTime).HasColumnName("ClassEndTime");
            entity.Property(t => t.ClassBeginDate).HasColumnName("ClassBeginDate");
            entity.Property(t => t.ClassEndDate).HasColumnName("ClassEndDate");
            entity.Property(t => t.TermId).HasColumnName("TermId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.CourseLevelId).HasColumnName("CourseLevelId");
           entity.Property(t => t.ClassRoomId).HasColumnName("ClassRoomId");
           entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
           entity.Property(t => t.LessonType).HasColumnName("LessonType");
           entity.Property(t => t.LessonCount).HasColumnName("LessonCount");
           entity.Property(t => t.BusinessType).HasColumnName("BusinessType");
           entity.Property(t => t.BusinessId).HasColumnName("BusinessId");
           entity.Property(t => t.Status).HasColumnName("Status");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
