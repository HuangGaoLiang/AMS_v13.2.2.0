
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblDatSchoolTime> TblDatSchoolTime { get; set; }
        public virtual DbSet<TblDatTerm> TblDatTerm { get; set; }
        public virtual DbSet<TblDatClassRoom> TblDatClassRoom { get; set; }
        public virtual DbSet<TblDatClass> TblDatClass { get; set; }
        public virtual DbSet<TblDatRoomCourse> TblDatRoomCourse { get; set; }
        public virtual DbSet<TblDatHoliday> TblDatHoliday { get; set; }
        public virtual DbQuery<ViewRoomCourse> ViewRoomCourse { get; set; }
        public virtual DbSet<TblDatCourseLevel> TblDatCourseLevel { get; set; }
        public virtual DbSet<TblDatCourseLevelMiddle> TblDatCourseLevelMiddle { get; set; }
        public virtual DbSet<TblDatCourse> TblDatCourse { get; set; }
        public virtual DbSet<TblDatSchoolCourse> TblDatSchoolCourse { get; set; }
        public virtual DbQuery<ViewCourseLevelMiddle> ViewCourseLevelMiddles { get; set; }
        public virtual DbSet<TblDatPrintCounter> TblDatPrintCounters { get; set; }
        public virtual DbSet<TblDatPrintCounter> TblDatPrintCounter { get; set; }
        public virtual DbSet<TblDatCost> TblDatCost { get; set; }
        public virtual DbSet<TblDatCostType> TblDatCostType { get; set; }
        public virtual DbSet<TblDatCostMiddle> TblDatCostMiddle { get; set; }
        public virtual DbSet<TblDatAttchment> TblDatAttchment { get; set; }
        public virtual DbSet<TblDatOperationLog> TblDatOperationLog { get; set; }
        public virtual DbSet<TblDatFeedback> TblDatFeedback { get; set; }
        public virtual DbSet<TblDatBusinessConfig> TblDatBusinessConfig { get; set; }
        public virtual DbSet<TblDatClassReplenishWeek> TblDatClassReplenishWeek { get; set; }
        public void ApplyConfigurationDatum(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblDatSchoolTimeMap());
            modelBuilder.ApplyConfiguration(new TblDatCostTypeMap());
            modelBuilder.ApplyConfiguration(new TblDatCostMiddleMap());
            modelBuilder.ApplyConfiguration(new TblDatTermMap());
            modelBuilder.ApplyConfiguration(new TblDatClassRoomMap());
            modelBuilder.ApplyConfiguration(new TblDatClassMap());
            modelBuilder.ApplyConfiguration(new TblDatRoomCourseMap());
            modelBuilder.ApplyConfiguration(new TblDatHolidayMap());
            modelBuilder.ApplyConfiguration(new TblDatCourseLevelMap());
            modelBuilder.ApplyConfiguration(new TblDatCourseLevelMiddleMap());
            modelBuilder.ApplyConfiguration(new TblDatCourseMap());
            modelBuilder.ApplyConfiguration(new TblDatSchoolCourseMap());
            modelBuilder.ApplyConfiguration(new TblDatPrintCounterMap());
            modelBuilder.ApplyConfiguration(new TblDatCostMap());
            modelBuilder.ApplyConfiguration(new TblDatAttchmentMap());
            modelBuilder.ApplyConfiguration(new TblDatOperationLogMap());
            modelBuilder.ApplyConfiguration(new TblDatFeedbackMap());
            modelBuilder.ApplyConfiguration(new TblDatBusinessConfigMap());
            modelBuilder.ApplyConfiguration(new TblDatClassReplenishWeekMap());
        }
    }
}
