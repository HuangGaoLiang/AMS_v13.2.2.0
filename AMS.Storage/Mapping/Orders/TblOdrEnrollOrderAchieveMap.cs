/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 业绩归属人
     /// </summary>
   class TblOdrEnrollOrderAchieveMap : IEntityTypeConfiguration<TblOdrEnrollOrderAchieve>
   {
        void IEntityTypeConfiguration<TblOdrEnrollOrderAchieve>.Configure(EntityTypeBuilder<TblOdrEnrollOrderAchieve> entity)
        {
           // Table
           entity.ToTable("TblOdrEnrollOrderAchieve");

           // Primary Key
           entity.HasKey(e => e.EnrollOrderAchieveId);

           // Properties
           entity.Property(e => e.EnrollOrderAchieveId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PersonalId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Proportion)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.EnrollOrderAchieveId).HasColumnName("EnrollOrderAchieveId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.EnrollOrderId).HasColumnName("EnrollOrderId");
           entity.Property(t => t.PersonalId).HasColumnName("PersonalId");
           entity.Property(t => t.Proportion).HasColumnName("Proportion");
        }
   }
}
