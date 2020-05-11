/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：校区写生排课表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    class TblTimLifeClassMap : IEntityTypeConfiguration<TblTimLifeClass>
   {
        void IEntityTypeConfiguration<TblTimLifeClass>.Configure(EntityTypeBuilder<TblTimLifeClass> entity)
        {
           // Table
           entity.ToTable("TblTimLifeClass");

           // Primary Key
           entity.HasKey(e => e.LifeClassId);

           // Properties
           entity.Property(e => e.LifeClassId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TermId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.LifeClassCode)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Title)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Place)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.TeacherId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassBeginTime)
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassEndTime)
                 .IsUnicode(false) ;

           entity.Property(e => e.UseLessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.LifeClassId).HasColumnName("LifeClassId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.TermId).HasColumnName("TermId");
           entity.Property(t => t.LifeClassCode).HasColumnName("LifeClassCode");
           entity.Property(t => t.Title).HasColumnName("Title");
           entity.Property(t => t.Place).HasColumnName("Place");
           entity.Property(t => t.TeacherId).HasColumnName("TeacherId");
           entity.Property(t => t.ClassBeginTime).HasColumnName("ClassBeginTime");
           entity.Property(t => t.ClassEndTime).HasColumnName("ClassEndTime");
           entity.Property(t => t.UseLessonCount).HasColumnName("UseLessonCount");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
