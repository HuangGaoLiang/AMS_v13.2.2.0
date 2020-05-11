/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
     /// <summary>
     /// 退费订单-游学营
     /// </summary>
   class TblOdrCampOrderMap : IEntityTypeConfiguration<TblOdrCampOrder>
   {
        void IEntityTypeConfiguration<TblOdrCampOrder>.Configure(EntityTypeBuilder<TblOdrCampOrder> entity)
        {
           // Table
           entity.ToTable("TblOdrCampOrder");

           // Primary Key
           entity.HasKey(e => e.RefundOrderId);

           // Properties
           entity.Property(e => e.RefundOrderId)
                 .IsRequired()
                 .IsUnicode(false) ;

           // Column
           entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
        }
   }
}
