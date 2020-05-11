/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 课程级别设置中间表
     /// </summary>
   class TblDatCourseLevelMiddleMap : IEntityTypeConfiguration<TblDatCourseLevelMiddle>
   {
        void IEntityTypeConfiguration<TblDatCourseLevelMiddle>.Configure(EntityTypeBuilder<TblDatCourseLevelMiddle> entity)
        {
           // Table
           entity.ToTable("TblDatCourseLevelMiddle");

           // Primary Key
           entity.HasKey(e => e.CourseLevelMiddleId);

           // Properties
           entity.Property(e => e.CourseLevelMiddleId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseLevelId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BeginAge)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EndAge)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Duration)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.CourseLevelMiddleId).HasColumnName("CourseLevelMiddleId");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.CourseLevelId).HasColumnName("CourseLevelId");
           entity.Property(t => t.BeginAge).HasColumnName("BeginAge");
           entity.Property(t => t.EndAge).HasColumnName("EndAge");
           entity.Property(t => t.Duration).HasColumnName("Duration");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
