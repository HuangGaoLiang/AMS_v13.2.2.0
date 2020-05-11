/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 老师上课时间表
     /// </summary>
   class TblTimLessonTeacherMap : IEntityTypeConfiguration<TblTimLessonTeacher>
   {
        void IEntityTypeConfiguration<TblTimLessonTeacher>.Configure(EntityTypeBuilder<TblTimLessonTeacher> entity)
        {
           // Table
           entity.ToTable("TblTimLessonTeacher");

           // Primary Key
           entity.HasKey(e => e.LessonTeacherId);

           // Properties
           entity.Property(e => e.LessonTeacherId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TeacherId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimeBegin)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimeEnd)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AttendStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.LessonTeacherId).HasColumnName("LessonTeacherId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.ClassDate).HasColumnName("ClassDate");
           entity.Property(t => t.ClassTimeBegin).HasColumnName("ClassTimeBegin");
           entity.Property(t => t.ClassTimeEnd).HasColumnName("ClassTimeEnd");
           entity.Property(t => t.AttendStatus).HasColumnName("AttendStatus");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
