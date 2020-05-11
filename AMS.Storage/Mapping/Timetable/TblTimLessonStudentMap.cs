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
   class TblTimLessonStudentMap : IEntityTypeConfiguration<TblTimLessonStudent>
   {
        void IEntityTypeConfiguration<TblTimLessonStudent>.Configure(EntityTypeBuilder<TblTimLessonStudent> entity)
        {
           // Table
           entity.ToTable("TblTimLessonStudent");

           // Primary Key
           entity.HasKey(e => e.LessonStudentId);

           // Properties
           entity.Property(e => e.LessonStudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.LessonId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AttendStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.LessonStudentId).HasColumnName("LessonStudentId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.LessonId).HasColumnName("LessonId");
           entity.Property(t => t.AttendStatus).HasColumnName("AttendStatus");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
