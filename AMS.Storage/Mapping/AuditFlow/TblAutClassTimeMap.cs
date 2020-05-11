/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 班级上课时间表
     /// </summary>
   class TblAutClassTimeMap : IEntityTypeConfiguration<TblAutClassTime>
   {
        void IEntityTypeConfiguration<TblAutClassTime>.Configure(EntityTypeBuilder<TblAutClassTime> entity)
        {
           // Table
           entity.ToTable("TblAutClassTime");

           // Primary Key
           entity.HasKey(e => e.AutClassTimeId);

           // Properties
           entity.Property(e => e.AutClassTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.AuditId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.DataStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.AutClassTimeId).HasColumnName("AutClassTimeId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.ClassTimeId).HasColumnName("ClassTimeId");
           entity.Property(t => t.AuditId).HasColumnName("AuditId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.SchoolTimeId).HasColumnName("SchoolTimeId");
           entity.Property(t => t.DataStatus).HasColumnName("DataStatus");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
