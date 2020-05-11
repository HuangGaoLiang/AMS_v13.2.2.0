using AMS.Models;
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblTimClassTime> TblTimClassTime { get; set; }
        public virtual DbSet<TblTimLesson> TblTimLesson { get; set; }
        public virtual DbSet<TblTimLessonTeacher> TblTimLessonTeacher { get; set; }
        public virtual DbSet<TblTimLessonStudent> TblTimLessonStudent { get; set; }
        public virtual DbSet<TblTimLessonProcess> TblTimLessonProcess { get; set; }
        public virtual DbSet<TblTimMakeLesson> TblTimMakeLesson { get; set; }
        public virtual DbSet<TblTimLifeClass> TblTimLifeClass { get; set; }
        public virtual DbSet<TblTimChangeClass> TblTimChangeClass { get; set; }
        public virtual DbSet<TblTimAdjustLesson> TblTimAdjustLesson { get; set; }
        public virtual DbQuery<ViewStudentAttendance> ViewStudentAttendance { get; set; }
        public virtual DbQuery<ViewTimStudentStudyRecord> ViewTimStudentRecord { get; set; }
        public virtual DbQuery<ViewTimLifeClass> ViewTimLifeClass { get; set; }
        public virtual DbQuery<ViewTimLifeClassStudent> ViewTimLifeClassStudent { get; set; }
        public virtual DbQuery<ViewTimeLessonClass> ViewTimeLessonClass { get; set; }
        public virtual DbQuery<ViewTimLessonStudent> ViewTimLessonStudent { get; set; }
        public virtual DbQuery<ViewTimChangeClass> ViewTimChangeClass { get; set; }
        public virtual DbQuery<ViewStudentScanCodeAttend> ViewStudentScanCodeAttend { get; set; }
        public virtual DbQuery<ViewCancelMakeLesson> ViewCancelMakeLesson { get; set; }
        public virtual DbQuery<ViewTeacherNoAttendLesson> ViewTeacherNoAttendLesson { get; set; }
        public virtual DbQuery<ViewClassLesson> ViewClassLesson { get; set; }
        public virtual DbQuery<ViewTotalClassStudentAbnormalState> ViewTotalClassStudentAbnormalState { get; set; }
        public virtual DbQuery<ViewTeacherClassDate> View_TeacherClassDate { get; set; }
        public virtual DbSet<TblTimReplenishLesson> TblTimReplenishLesson { get; set; }
        public virtual DbQuery<ViewReplenishWeek> ViewReplenishWeek { get; set; }
        public virtual DbQuery<ViewTimAttendLesson> ViewTimAttendLesson { get; set; }
        public virtual DbQuery<ViewStudentReplenishLesson> ViewStudentReplenishLessons { get; set; }
        public virtual DbQuery<ViewCompleteStudentAttendance> ViewCompleteStudentAttendance { get; set; }
        public virtual DbQuery<ViewClassTeacherDate> ViewClassTeacherDate { get; set; }
        public virtual DbQuery<ViewStudentTimeLess> ViewStudentTimeLess { get; set; }
        public virtual DbQuery<ViewTimAdjustLesson> ViewTimAdjustLesson { get; set; }
        public virtual DbQuery<ViewChangeClassTime> ViewChangeClassTime { get; set; }

        public void ApplyConfigurationTimetable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblTimMakeLessonMap());
            modelBuilder.ApplyConfiguration(new TblTimClassTimeMap());
            modelBuilder.ApplyConfiguration(new TblTimLessonMap());
            modelBuilder.ApplyConfiguration(new TblTimLessonTeacherMap());
            modelBuilder.ApplyConfiguration(new TblTimLessonStudentMap());
            modelBuilder.ApplyConfiguration(new TblTimLessonProcessMap());
            modelBuilder.ApplyConfiguration(new TblTimLifeClassMap());
            modelBuilder.ApplyConfiguration(new TblTimChangeClassMap());
            modelBuilder.ApplyConfiguration(new TblTimReplenishLessonMap());
            modelBuilder.ApplyConfiguration(new TblTimAdjustLessonMap());
        }
    }
}
