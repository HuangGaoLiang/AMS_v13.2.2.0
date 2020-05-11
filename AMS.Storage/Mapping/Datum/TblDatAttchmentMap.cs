/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 
     /// </summary>
   class TblDatAttchmentMap : IEntityTypeConfiguration<TblDatAttchment>
   {
        void IEntityTypeConfiguration<TblDatAttchment>.Configure(EntityTypeBuilder<TblDatAttchment> entity)
        {
           // Table
           entity.ToTable("TblDatAttchment");

           // Primary Key
           entity.HasKey(e => e.AttchmentId);

           // Properties
           entity.Property(e => e.AttchmentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.AttchmentType)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Url)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Name)
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.AttchmentId).HasColumnName("AttchmentId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.AttchmentType).HasColumnName("AttchmentType");
           entity.Property(t => t.BusinessId).HasColumnName("BusinessId");
           entity.Property(t => t.Url).HasColumnName("Url");
           entity.Property(t => t.Name).HasColumnName("Name");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
