/*此代码由生成工具字段生成，生成时间2019/3/14 16:29:38 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：TblFinAttendanceConfirm(考勤确认表)
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-14</para>
    /// </summary>
    class TblFinAttendanceConfirmMap : IEntityTypeConfiguration<TblFinAttendanceConfirm>
   {
        void IEntityTypeConfiguration<TblFinAttendanceConfirm>.Configure(EntityTypeBuilder<TblFinAttendanceConfirm> entity)
        {
           // Table
           entity.ToTable("TblFinAttendanceConfirm");

           // Primary Key
           entity.HasKey(e => e.AttendanceConfirmId);

           // Properties
           entity.Property(e => e.AttendanceConfirmId)
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

           entity.Property(e => e.Month)
                 .IsUnicode(false) ;

           entity.Property(e => e.TeacherSignUrl)
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.AttendanceConfirmId).HasColumnName("AttendanceConfirmId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.Month).HasColumnName("Month");
           entity.Property(t => t.TeacherSignUrl).HasColumnName("TeacherSignUrl");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
