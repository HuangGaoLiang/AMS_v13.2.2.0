/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMS.Storage.Models.Storage.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    class TblCstStudentMap : IEntityTypeConfiguration<TblCstStudent>
    {
        void IEntityTypeConfiguration<TblCstStudent>.Configure(EntityTypeBuilder<TblCstStudent> entity)
        {
            // Table
            entity.ToTable("TblCstStudent");

            // Primary Key
            entity.HasKey(e => e.StudentId);

            // Properties
            entity.Property(e => e.StudentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.StudentNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.StudentName)
                 .IsRequired()
                 .HasMaxLength(50)
                 .IsUnicode(false);

            entity.Property(e => e.Sex)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Birthday)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.HeadFaceUrl)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.CurrentSchool)
                  .IsRequired()
                  .HasMaxLength(100)
                  .IsUnicode(false);

            entity.Property(e => e.AreaId)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.HomeAddress)
                  .IsRequired()
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.HomeAddressFormat)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.IDType)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.IDNumber)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.LinkMobile)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.LinkMail)
                  .IsRequired()
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.ContactPerson)
                  .IsRequired()
                  .HasMaxLength(4000)
                  .IsUnicode(false);

            entity.Property(e => e.ContactPersonMobile)
                  .IsRequired()
                  .HasMaxLength(1000)
                  .IsUnicode(false);

            entity.Property(e => e.CustomerFrom)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.ParentId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Remark)
                .IsRequired()
                .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.StudentId).HasColumnName("StudentId");
            entity.Property(t => t.StudentNo).HasColumnName("StudentNo");
            entity.Property(t => t.StudentName).HasColumnName("StudentName");
            entity.Property(t => t.Sex).HasColumnName("Sex");
            entity.Property(t => t.Birthday).HasColumnName("Birthday");
            entity.Property(t => t.HeadFaceUrl).HasColumnName("HeadFaceUrl");
            entity.Property(t => t.CurrentSchool).HasColumnName("CurrentSchool");
            entity.Property(t => t.AreaId).HasColumnName("AreaId");
            entity.Property(t => t.HomeAddress).HasColumnName("HomeAddress");
            entity.Property(t => t.HomeAddressFormat).HasColumnName("HomeAddressFormat");
            entity.Property(t => t.IDType).HasColumnName("IDType");
            entity.Property(t => t.IDNumber).HasColumnName("IDNumber");
            entity.Property(t => t.LinkMobile).HasColumnName("LinkMobile");
            entity.Property(t => t.LinkMail).HasColumnName("LinkMail");
            entity.Property(t => t.ContactPerson).HasColumnName("ContactPerson");
            entity.Property(t => t.ContactPersonMobile).HasColumnName("ContactPersonMobile");
            entity.Property(t => t.CustomerFrom).HasColumnName("CustomerFrom");
            entity.Property(t => t.ParentId).HasColumnName("ParentId");
            entity.Property(t => t.Remark).HasColumnName("Remark");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
