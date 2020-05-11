/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 排课表
     /// </summary>
   class TblTimMakeLessonMap : IEntityTypeConfiguration<TblTimMakeLesson>
   {
        void IEntityTypeConfiguration<TblTimMakeLesson>.Configure(EntityTypeBuilder<TblTimMakeLesson> entity)
        {
           // Table
           entity.ToTable("TblTimMakeLesson");

           // Primary Key
           entity.HasKey(e => e.MakeLessonId);

           // Properties
           entity.Property(e => e.MakeLessonId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollOrderItemId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimes)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.IsConfirm)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.FirstClassTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.MakeLessonId).HasColumnName("MakeLessonId");
           entity.Property(t => t.EnrollOrderItemId).HasColumnName("EnrollOrderItemId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.ClassTimes).HasColumnName("ClassTimes");
           entity.Property(t => t.IsConfirm).HasColumnName("IsConfirm");
           entity.Property(t => t.FirstClassTime).HasColumnName("FirstClassTime");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
