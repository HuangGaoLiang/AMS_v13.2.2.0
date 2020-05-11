/*此代码由生成工具字段生成，生成时间17/11/2018 09:43:51 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 描    述：TblFinOrderHandover(订单交接核对主表)
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    class TblFinOrderHandoverMap : IEntityTypeConfiguration<TblFinOrderHandover>
   {
        void IEntityTypeConfiguration<TblFinOrderHandover>.Configure(EntityTypeBuilder<TblFinOrderHandover> entity)
        {
           // Table
           entity.ToTable("TblFinOrderHandover");

           // Primary Key
           entity.HasKey(e => e.OrderHandoverId);

           // Properties
           entity.Property(e => e.OrderHandoverId)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.SchoolId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.PersonalId)
                 .IsRequired()
                 .HasMaxLength(32) 
                 .IsUnicode(false) ;

           entity.Property(e => e.PersonalName)
                 .IsRequired()
                 .HasMaxLength(50) 
                 .IsUnicode(false) ;

           entity.Property(e => e.DayIncomeAmout)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.InBankAmount)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.ReceiptNumber)
                 .IsRequired()
                 .IsUnicode(false) ;

           entity.Property(e => e.HandoverUrl)
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.RecipientUrl)
                 .HasMaxLength(200) 
                 .IsUnicode(false) ;

           entity.Property(e => e.Remark)
                 .HasMaxLength(500) 
                 .IsUnicode(false) ;

           entity.Property(e => e.HandoverDate)
                 .IsRequired()
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
           entity.Property(t => t.OrderHandoverId).HasColumnName("OrderHandoverId");
           entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
           entity.Property(t => t.PersonalId).HasColumnName("PersonalId");
           entity.Property(t => t.PersonalName).HasColumnName("PersonalName");
           entity.Property(t => t.DayIncomeAmout).HasColumnName("DayIncomeAmout");
           entity.Property(t => t.InBankAmount).HasColumnName("InBankAmount");
           entity.Property(t => t.ReceiptNumber).HasColumnName("ReceiptNumber");
           entity.Property(t => t.HandoverUrl).HasColumnName("HandoverUrl");
           entity.Property(t => t.RecipientUrl).HasColumnName("RecipientUrl");
           entity.Property(t => t.Remark).HasColumnName("Remark");
           entity.Property(t => t.HandoverDate).HasColumnName("HandoverDate");
           entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
           entity.Property(t => t.CreatorId).HasColumnName("CreatorId");
           entity.Property(t => t.CreatorName).HasColumnName("CreatorName");
        }
   }
}
