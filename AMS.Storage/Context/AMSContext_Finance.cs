using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblFinOrderHandover> TblFinOrderHandover { get; set; }
        public virtual DbSet<TblFinOrderHandoverDetail> TblFinOrderHandoverDetail { get; set; }
        public virtual DbQuery<ViewFinOrderHandoverDetailSummary> ViewFinOrderHandoverDetailSummary { get; set; }

        /// <summary>
        /// 考勤确认信息上下文实体
        /// </summary>
        public virtual DbSet<TblFinAttendanceConfirm> TblFinAttendanceConfirm { get; set; }

        public void ApplyConfigurationFinance(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblFinOrderHandoverMap());
            modelBuilder.ApplyConfiguration(new TblFinOrderHandoverDetailMap());
            modelBuilder.ApplyConfiguration(new TblFinAttendanceConfirmMap());
        }
    }
}
