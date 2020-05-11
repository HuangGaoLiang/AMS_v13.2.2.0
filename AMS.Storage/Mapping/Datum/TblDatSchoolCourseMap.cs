/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 校区授权课程
     /// </summary>
   class TblDatSchoolCourseMap : IEntityTypeConfiguration<TblDatSchoolCourse>
   {
        void IEntityTypeConfiguration<TblDatSchoolCourse>.Configure(EntityTypeBuilder<TblDatSchoolCourse> entity)
        {
           // Table
           entity.ToTable("TblDatSchoolCourse");

           // Primary Key
           entity.HasKey(e => e.SchoolCourseId);

           // Properties
           entity.Property(e => e.SchoolCourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.SchoolCourseId).HasColumnName("SchoolCourseId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
