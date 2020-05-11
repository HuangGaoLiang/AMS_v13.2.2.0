/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 班级课程表
     /// </summary>
   class TblTimClassTimeMap : IEntityTypeConfiguration<TblTimClassTime>
   {
        void IEntityTypeConfiguration<TblTimClassTime>.Configure(EntityTypeBuilder<TblTimClassTime> entity)
        {
           // Table
           entity.ToTable("TblTimClassTime");

           // Primary Key
           entity.HasKey(e => e.ClassTimeId);

           // Properties
           entity.Property(e => e.ClassTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolTimeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.ClassTimeId).HasColumnName("ClassTimeId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.ClassId).HasColumnName("ClassId");
           entity.Property(t => t.SchoolTimeId).HasColumnName("SchoolTimeId");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
