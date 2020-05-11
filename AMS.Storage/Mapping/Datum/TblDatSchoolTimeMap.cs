/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 上课时间段表
     /// </summary>
   class TblDatSchoolTimeMap : IEntityTypeConfiguration<TblDatSchoolTime>
   {
        void IEntityTypeConfiguration<TblDatSchoolTime>.Configure(EntityTypeBuilder<TblDatSchoolTime> entity)
        {
           // Table
           entity.ToTable("TblDatSchoolTime");

           // Primary Key
           entity.HasKey(e => e.SchoolTimeId);

           // Properties
           entity.Property(e => e.SchoolTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TermId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.WeekDay)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Duration)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.BeginTime)
                 .IsRequired()
                 .HasMaxLength(10) 
                 .IsUnicode(false) ;

           entity.Property(e => e.EndTime)
                 .IsRequired()
                 .HasMaxLength(10) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimeNo)
                 .IsRequired()
                 .HasMaxLength(20) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.SchoolTimeId).HasColumnName("SchoolTimeId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.TermId).HasColumnName("TermId");
           entity.Property(t => t.WeekDay).HasColumnName("WeekDay");
           entity.Property(t => t.Duration).HasColumnName("Duration");
           entity.Property(t => t.BeginTime).HasColumnName("BeginTime");
           entity.Property(t => t.EndTime).HasColumnName("EndTime");
           entity.Property(t => t.ClassTimeNo).HasColumnName("ClassTimeNo");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
