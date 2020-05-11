/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 
     /// </summary>
   class TblHssPassportMap : IEntityTypeConfiguration<TblHssPassport>
   {
        void IEntityTypeConfiguration<TblHssPassport>.Configure(EntityTypeBuilder<TblHssPassport> entity)
        {
           // Table
           entity.ToTable("TblHssPassport");

           // Primary Key
           entity.HasKey(e => e.PassporId);

           // Properties
           entity.Property(e => e.PassporId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UserCode)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OpenId)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

            entity.Property(e => e.UnionId)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CurrentLoginIp)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CurrentLoginDate)
                 .IsUnicode(false) ;

           entity.Property(e => e.LastLoginIp)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.LastLoginDate)
                 .IsUnicode(false) ;

           entity.Property(e => e.LoginTimes)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.PassporId).HasColumnName("PassporId");
           entity.Property(t => t.UserCode).HasColumnName("UserCode");
           entity.Property(t => t.OpenId).HasColumnName("OpenId");
           entity.Property(t => t.CurrentLoginIp).HasColumnName("CurrentLoginIp");
           entity.Property(t => t.CurrentLoginDate).HasColumnName("CurrentLoginDate");
           entity.Property(t => t.LastLoginIp).HasColumnName("LastLoginIp");
           entity.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
           entity.Property(t => t.LoginTimes).HasColumnName("LoginTimes");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
