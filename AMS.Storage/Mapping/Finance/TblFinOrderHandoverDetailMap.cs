/*此代码由生成工具字段生成，生成时间17/11/2018 20:55:48 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：TblFinOrderHandoverDetail(订单交接核对明细)
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    class TblFinOrderHandoverDetailMap : IEntityTypeConfiguration<TblFinOrderHandoverDetail>
   {
        void IEntityTypeConfiguration<TblFinOrderHandoverDetail>.Configure(EntityTypeBuilder<TblFinOrderHandoverDetail> entity)
        {
           // Table
           entity.ToTable("TblFinOrderHandoverDetail");

           // Primary Key
           entity.HasKey(e => e.OrderHandoverDetailId);

           // Properties
           entity.Property(e => e.OrderHandoverDetailId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderHandoverId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.StudentId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PersonalId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderNo)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.OrderTradeType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.UseBalanceAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.TotalDiscountFee)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PayType)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PayAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.PayDate)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.HandoverStatus)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .IsRequired()
                 .HasMaxLength(400) 
                 .IsUnicode(false) ;

           entity.Property(e => e.HandoverDate)
                 .IsUnicode(false) ;

           entity.Property(e => e.CreateTime)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.CreatorId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.CreatorName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.OrderHandoverDetailId).HasColumnName("OrderHandoverDetailId");
           entity.Property(t => t.OrderHandoverId).HasColumnName("OrderHandoverId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.StudentId).HasColumnName("StudentId");
           entity.Property(t => t.PersonalId).HasColumnName("PersonalId");
           entity.Property(t => t.OrderId).HasColumnName("OrderId");
           entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
           entity.Property(t => t.OrderTradeType).HasColumnName("OrderTradeType");
           entity.Property(t => t.UseBalanceAmount).HasColumnName("UseBalanceAmount");
           entity.Property(t => t.TotalDiscountFee).HasColumnName("TotalDiscountFee");
           entity.Property(t => t.PayType).HasColumnName("PayType");
           entity.Property(t => t.PayAmount).HasColumnName("PayAmount");
           entity.Property(t => t.PayDate).HasColumnName("PayDate");
           entity.Property(t => t.HandoverStatus).HasColumnName("HandoverStatus");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.HandoverDate).HasColumnName("HandoverDate");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.CreatorId).HasColumnName("CreatorId");
           entity.Property(t => t.CreatorName).HasColumnName("CreatorName");
        }
   }
}
