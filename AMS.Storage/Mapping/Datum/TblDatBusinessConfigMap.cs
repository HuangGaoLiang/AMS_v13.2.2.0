/*此代码由生成工具字段生成，生成时间2019/3/13 10:52:09 */
using System;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 
     /// </summary>
   class TblDatBusinessConfigMap : IEntityTypeConfiguration<TblDatBusinessConfig>
   {
        void IEntityTypeConfiguration<TblDatBusinessConfig>.Configure(EntityTypeBuilder<TblDatBusinessConfig> entity)
        {
           // Table
           entity.ToTable("TblDatBusinessConfig");

            // Primary Key
            entity.HasKey(e => e.BusinessConfigKey);

            // Properties
            entity.Property(e => e.BusinessConfigKey)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CompanyId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessConfigName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BusinessConfigData)
                 .IsRequired()
                 .HasMaxLength(-1) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.BusinessConfigKey).HasColumnName("BusinessConfigKey");
           entity.Property(t => t.CompanyId).HasColumnName("CompanyId");
           entity.Property(t => t.BusinessConfigName).HasColumnName("BusinessConfigName");
           entity.Property(t => t.BusinessConfigData).HasColumnName("BusinessConfigData");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
