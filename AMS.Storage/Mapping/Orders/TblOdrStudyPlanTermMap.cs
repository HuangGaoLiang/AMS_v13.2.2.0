/*此代码由生成工具字段生成，生成时间08/11/2018 15:37:10 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：报名学习计划学期
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    class TblOdrStudyPlanTermMap : IEntityTypeConfiguration<TblOdrStudyPlanTerm>
   {
        void IEntityTypeConfiguration<TblOdrStudyPlanTerm>.Configure(EntityTypeBuilder<TblOdrStudyPlanTerm> entity)
        {
           // Table
           entity.ToTable("TblOdrStudyPlanTerm");

           // Primary Key
           entity.HasKey(e => e.StudyPlanTermId);

           // Properties
           entity.Property(e => e.StudyPlanTermId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TermTypeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TermTypeName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Year)
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

           entity.Property(e => e.BeginDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EndDate)
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

           // Column
           entity.Property(t => t.StudyPlanTermId).HasColumnName("StudyPlanTermId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.TermTypeId).HasColumnName("TermTypeId");
           entity.Property(t => t.TermTypeName).HasColumnName("TermTypeName");
           entity.Property(t => t.Year).HasColumnName("Year");
           entity.Property(t => t.Classes60).HasColumnName("Classes60");
           entity.Property(t => t.Classes90).HasColumnName("Classes90");
           entity.Property(t => t.Classes180).HasColumnName("Classes180");
           entity.Property(t => t.BeginDate).HasColumnName("BeginDate");
           entity.Property(t => t.EndDate).HasColumnName("EndDate");
           entity.Property(t => t.TuitionFee).HasColumnName("TuitionFee");
           entity.Property(t => t.MaterialFee).HasColumnName("MaterialFee");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
