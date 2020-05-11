using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
 

namespace Jerrisoft.Platform.Controllers
{
    public partial class MarketingContext
    {
 
         
        public virtual DbSet<TActivityType> TActivityType { get; set; }
      

        public void ApplyConfigurationActivity(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfiguration(new TActivityTypeMap());
            
        }
    }
    public class TActivityTypeMap : IEntityTypeConfiguration<TActivityType>
    {
        public TActivityTypeMap() { }
        void IEntityTypeConfiguration<TActivityType>.Configure(EntityTypeBuilder<TActivityType> entity)
        {
            entity.ToTable("T_ActivityType");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .ValueGeneratedNever();

            entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreateId)
                  .IsRequired()
               .HasMaxLength(50);
             
            entity.Property(e => e.CreateName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
              .IsRequired();

            entity.Property(e => e.UpdateId)
              .IsRequired(false)
              .HasMaxLength(50);

          

            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
              .IsRequired(false);
        }
    }

    /// <summary>
    /// 活动类型
    /// </summary>
    public partial class TActivityType
    {
        /// <summary>
        /// 主键
        /// </summary>
        //[Key]
        public string Id { get; set; }

        /// <summary>
        /// 活动类型名称
        /// </summary>
        public string TypeName { get; set; }
         
        public string CreateId { get; set; }
         
        public string CreateName { get; set; }
         
        public string UpdateId { get; set; }
         
        public DateTime? UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
