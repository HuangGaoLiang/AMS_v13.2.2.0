/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 学期
     /// </summary>
   class TblDatTermMap : IEntityTypeConfiguration<TblDatTerm>
   {
        void IEntityTypeConfiguration<TblDatTerm>.Configure(EntityTypeBuilder<TblDatTerm> entity)
        {
           // Table
           entity.ToTable("TblDatTerm");

           // Primary Key
           entity.HasKey(e => e.TermId);

           // Properties
           entity.Property(e => e.TermId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Year)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TermTypeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TermName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.BeginDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EndDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Classes60)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Classes90)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Classes180)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TuitionFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaterialFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UpdateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.TermId).HasColumnName("TermId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.Year).HasColumnName("Year");
           entity.Property(t => t.TermTypeId).HasColumnName("TermTypeId");
           entity.Property(t => t.TermName).HasColumnName("TermName");
           entity.Property(t => t.BeginDate).HasColumnName("BeginDate");
           entity.Property(t => t.EndDate).HasColumnName("EndDate");
           entity.Property(t => t.Classes60).HasColumnName("Classes60");
           entity.Property(t => t.Classes90).HasColumnName("Classes90");
           entity.Property(t => t.Classes180).HasColumnName("Classes180");
           entity.Property(t => t.TuitionFee).HasColumnName("TuitionFee");
           entity.Property(t => t.MaterialFee).HasColumnName("MaterialFee");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
   }
}
