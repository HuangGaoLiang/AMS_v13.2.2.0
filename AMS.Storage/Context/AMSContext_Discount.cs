using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    /// <summary>
    /// 优惠规则模块
    /// ---瞿琦 2018-10-27
    /// </summary>
    public partial class AMSContext
    {
        public virtual DbSet<TblDctCoupon> TblDctCoupon { get; set; }
        public virtual DbSet<TblDctCouponRule> TblDctCouponRule { get; set; }

        public void ApplyConfigurationDiscount(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblDctCouponRuleMap());
            modelBuilder.ApplyConfiguration(new TblDctCouponMap());
        }
    }
}
