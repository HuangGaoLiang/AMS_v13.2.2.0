/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 报名订单课程明细
     /// </summary>
   class TblOdrEnrollOrderItemMap : IEntityTypeConfiguration<TblOdrEnrollOrderItem>
   {
        void IEntityTypeConfiguration<TblOdrEnrollOrderItem>.Configure(EntityTypeBuilder<TblOdrEnrollOrderItem> entity)
        {
           // Table
           entity.ToTable("TblOdrEnrollOrderItem");

           // Primary Key
           entity.HasKey(e => e.EnrollOrderItemId);

           // Properties
           entity.Property(e => e.EnrollOrderItemId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.EnrollType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TermTypeId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Year)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CourseLevelId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Duration)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimes)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ClassTimesUse)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.DiscountFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TuitionFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaterialFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TuitionFeeReal)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.MaterialFeeReal)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Status)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.EnrollOrderItemId).HasColumnName("EnrollOrderItemId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.EnrollOrderId).HasColumnName("EnrollOrderId");
           entity.Property(t => t.EnrollType).HasColumnName("EnrollType");
           entity.Property(t => t.TermTypeId).HasColumnName("TermTypeId");
           entity.Property(t => t.Year).HasColumnName("Year");
           entity.Property(t => t.CourseId).HasColumnName("CourseId");
           entity.Property(t => t.CourseType).HasColumnName("CourseType");
           entity.Property(t => t.CourseLevelId).HasColumnName("CourseLevelId");
           entity.Property(t => t.Duration).HasColumnName("Duration");
           entity.Property(t => t.ClassTimes).HasColumnName("ClassTimes");
           entity.Property(t => t.ClassTimesUse).HasColumnName("ClassTimesUse");
           entity.Property(t => t.DiscountFee).HasColumnName("DiscountFee");
           entity.Property(t => t.TuitionFee).HasColumnName("TuitionFee");
           entity.Property(t => t.MaterialFee).HasColumnName("MaterialFee");
           entity.Property(t => t.TuitionFeeReal).HasColumnName("TuitionFeeReal");
           entity.Property(t => t.MaterialFeeReal).HasColumnName("MaterialFeeReal");
           entity.Property(t => t.Status).HasColumnName("Status");
        }
   }
}
