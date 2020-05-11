/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 教室课程表
     /// </summary>
   class TblDatRoomCourseMap : IEntityTypeConfiguration<TblDatRoomCourse>
   {
        void IEntityTypeConfiguration<TblDatRoomCourse>.Configure(EntityTypeBuilder<TblDatRoomCourse> entity)
        {
           // Table
           entity.ToTable("TblDatRoomCourse");

           // Primary Key
           entity.HasKey(e => e.RoomCourseId);

           // Properties
           entity.Property(e => e.RoomCourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassRoomId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaxWeekStage)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaxStageStudents)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.IsDisabled)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RoomCourseId).HasColumnName("RoomCourseId");
           entity.Property(t => t.ClassRoomId).HasColumnName("ClassRoomId");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.MaxWeekStage).HasColumnName("MaxWeekStage");
           entity.Property(t => t.MaxStageStudents).HasColumnName("MaxStageStudents");
           entity.Property(t => t.IsDisabled).HasColumnName("IsDisabled");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
