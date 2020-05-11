/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单-休学
     /// </summary>
   class TblOdrLeaveSchoolOrderMap : IEntityTypeConfiguration<TblOdrLeaveSchoolOrder>
   {
        void IEntityTypeConfiguration<TblOdrLeaveSchoolOrder>.Configure(EntityTypeBuilder<TblOdrLeaveSchoolOrder> entity)
        {
           // Table
           entity.ToTable("TblOdrLeaveSchoolOrder");

           // Primary Key
           entity.HasKey(e => e.RefundOrderId);

           // Properties
           entity.Property(e => e.RefundOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.LeaveTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ResumeTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TotalRefundLessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TotalUseLessonCount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Reason)
                 .IsRequired()
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.LeaveTime).HasColumnName("LeaveTime");
           entity.Property(t => t.ResumeTime).HasColumnName("ResumeTime");
           entity.Property(t => t.TotalRefundLessonCount).HasColumnName("TotalRefundLessonCount");
           entity.Property(t => t.TotalUseLessonCount).HasColumnName("TotalUseLessonCount");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.Reason).HasColumnName("Reason");
        }
   }
}
