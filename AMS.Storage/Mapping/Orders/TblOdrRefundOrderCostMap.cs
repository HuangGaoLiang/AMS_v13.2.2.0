/*此代码由生成工具字段生成，生成时间2018/11/16 10:50:41 */
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMS.Storage.Models;

namespace AMS.Storage.Mapping
{
    /// <summary>
    /// 退费订单-其他费用
    /// </summary>
    class TblOdrRefundOrderCostMap : IEntityTypeConfiguration<TblOdrRefundOrderCost>
    {
        void IEntityTypeConfiguration<TblOdrRefundOrderCost>.Configure(EntityTypeBuilder<TblOdrRefundOrderCost> entity)
        {
            // Table
            entity.ToTable("TblOdrRefundOrderCost");

            // Primary Key
            entity.HasKey(e => e.RefundClassCostId);

            // Properties
            entity.Property(e => e.RefundClassCostId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.RefundOrderId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.SchoolId).IsRequired().HasMaxLength(32).IsUnicode(false);

            entity.Property(e => e.CostId)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.Amount)
                  .IsRequired()
                  .IsUnicode(false);

            entity.Property(e => e.CreateTime)
                  .IsRequired()
                  .IsUnicode(false);

            // Column
            entity.Property(t => t.RefundClassCostId).HasColumnName("RefundClassCostId");
            entity.Property(t => t.RefundOrderId).HasColumnName("RefundOrderId");
            entity.Property(t => t.SchoolId).HasColumnName("SchoolId");
            entity.Property(t => t.CostId).HasColumnName("CostId");
            entity.Property(t => t.Amount).HasColumnName("Amount");
            entity.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
