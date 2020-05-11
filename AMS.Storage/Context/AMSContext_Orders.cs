using AMS.Models;
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblOdrDepositOrder> TblOdrDepositOrder { get; set; }
        public virtual DbSet<TblOdrStudyPlan> TblOdrStudyPlan { get; set; }
        public virtual DbSet<TblOdrStudyPlanTerm> TblOdrStudyPlanTerm { get; set; }
        public virtual DbSet<TblOdrEnrollOrder> TblOdrEnrollOrder { get; set; }
        public virtual DbSet<TblOdrEnrollOrderDiscount> TblOdrEnrollOrderDiscount { get; set; }
        public virtual DbSet<TblOdrEnrollOrderAchieve> TblOdrEnrollOrderAchieve { get; set; }
        public virtual DbSet<TblOdrLeaveSchoolOrder> TblOdrLeaveSchoolOrder { get; set; }
        public virtual DbSet<TblOdrLeaveClassOrder> TblOdrLeaveClassOrder { get; set; }
        public virtual DbSet<TblOdrCampOrder> TblOdrCampOrder { get; set; }
        public virtual DbSet<TblOdrRefundChangeSchoolOrder> TblOdrRefundChangeSchoolOrder { get; set; }
        public virtual DbSet<TblOdrEnrollOrderItem> TblOdrEnrollOrderItem { get; set; }
        public virtual DbSet<TblOdrRefundPay> TblOdrRefundPay { get; set; }
        public virtual DbSet<TblOdrRefundOrderCost> TblOdrRefundOrderCost { get; set; }
        public virtual DbSet<TblOdrRefundOrder> TblOdrRefundOrder { get; set; }
        public virtual DbSet<TblOdrRefundOrdeEnroll> TblOdrRefundOrdeEnroll { get; set; }
        public virtual DbQuery<ViewLeaveClassDetail> ViewLeaveClassDetail { get; set; }
        public virtual DbQuery<ViewChangeSchooolOrder> ViewTransferOrder { get; set; }

        public virtual DbQuery<ViewOdrEnrollOrder> ViewOdrEnrollOrder { get; set; }
        public virtual DbQuery<ViewDepositOrder> ViewDepositOrder { get; set; }
        public virtual DbQuery<ViewOrderObsoleteMonth> ViewOrderObsoleteMonth { get; set; }

        public virtual DbQuery<ViewLeaveSchoolOrder> ViewLeaveSchoolOrder { get; set; }

        public void ApplyConfigurationOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblOdrStudyPlanMap());
            modelBuilder.ApplyConfiguration(new TblOdrStudyPlanTermMap());
            modelBuilder.ApplyConfiguration(new TblOdrEnrollOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrEnrollOrderDiscountMap());
            modelBuilder.ApplyConfiguration(new TblOdrEnrollOrderAchieveMap());
            modelBuilder.ApplyConfiguration(new TblOdrLeaveSchoolOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrLeaveClassOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrCampOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrRefundChangeSchoolOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrEnrollOrderItemMap());
            modelBuilder.ApplyConfiguration(new TblOdrRefundPayMap());
            modelBuilder.ApplyConfiguration(new TblOdrRefundOrderCostMap());
            modelBuilder.ApplyConfiguration(new TblOdrRefundOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrDepositOrderMap());
            modelBuilder.ApplyConfiguration(new TblOdrRefundOrdeEnrollMap());
        }
    }
}
