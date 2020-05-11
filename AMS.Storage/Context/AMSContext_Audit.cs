using AMS.Models;
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using YMM.Storage.Mapping;
using YMM.Storage.Models;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblAutAudit> TblAutAudit { get; set; }
        public virtual DbSet<TblAutClass> TblAutClass { get; set; }
        public virtual DbSet<TblAutClassTime> TblAutClassTime { get; set; }
        public virtual DbSet<TblAutTerm> TblAutTerm { get; set; }
        public virtual DbSet<TblAutCouponRule> TblAutCouponRule { get; set; }

        public void ApplyConfigurationAudit(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblAutAuditMap());
            modelBuilder.ApplyConfiguration(new TblAutTermMap());
            modelBuilder.ApplyConfiguration(new TblAutClassMap());
            modelBuilder.ApplyConfiguration(new TblAutClassTimeMap());
            modelBuilder.ApplyConfiguration(new TblAutCouponRuleMap());
        }
    }
}
