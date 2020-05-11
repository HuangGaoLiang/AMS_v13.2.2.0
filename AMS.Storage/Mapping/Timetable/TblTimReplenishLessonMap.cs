/*此代码由生成工具字段生成，生成时间2019/3/7 16:44:09 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 补课信息表
     /// </summary>
   class TblTimReplenishLessonMap : IEntityTypeConfiguration<TblTimReplenishLesson>
   {
        void IEntityTypeConfiguration<TblTimReplenishLesson>.Configure(EntityTypeBuilder<TblTimReplenishLesson> entity)
        {
           // Table
           entity.ToTable("TblTimReplenishLesson");

           // Primary Key
           entity.HasKey(e => e.ReplenishLessonId);

           // Properties
           entity.Property(e => e.ReplenishLessonId)
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

           entity.Property(e => e.RootLessonId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ParentLessonId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AttendStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AttendDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ReplenishCode)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AttendUserType)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.ReplenishLessonId).HasColumnName("ReplenishLessonId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.LessonId).HasColumnName("LessonId");
           entity.Property(t => t.RootLessonId).HasColumnName("RootLessonId");
           entity.Property(t => t.ParentLessonId).HasColumnName("ParentLessonId");
           entity.Property(t => t.AttendStatus).HasColumnName("AttendStatus");
           entity.Property(t => t.AttendDate).HasColumnName("AttendDate");
           entity.Property(t => t.ReplenishCode).HasColumnName("ReplenishCode");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
           entity.Property(t => t.AttendUserType).HasColumnName("AttendUserType");
        }
   }
}
