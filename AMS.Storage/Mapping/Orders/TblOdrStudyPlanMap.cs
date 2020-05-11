/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：报名学习计划
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    class TblOdrStudyPlanMap : IEntityTypeConfiguration<TblOdrStudyPlan>
   {
        void IEntityTypeConfiguration<TblOdrStudyPlan>.Configure(EntityTypeBuilder<TblOdrStudyPlan> entity)
        {
           // Table
           entity.ToTable("TblOdrStudyPlan");

           // Primary Key
           entity.HasKey(e => e.StudyPlanId);

           // Properties
           entity.Property(e => e.StudyPlanId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseLevelId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseLevelName)
                 .IsRequired()
                 .HasMaxLength(100) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Age)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Duration)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PriorityLevel)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.StudyPlanId).HasColumnName("StudyPlanId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.CourseName).HasColumnName("CourseName");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.CourseLevelId).HasColumnName("CourseLevelId");
           entity.Property(t => t.CourseLevelName).HasColumnName("CourseLevelName");
           entity.Property(t => t.Age).HasColumnName("Age");
           entity.Property(t => t.Duration).HasColumnName("Duration");
           entity.Property(t => t.PriorityLevel).HasColumnName("PriorityLevel");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
   }
}
