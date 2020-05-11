using AMS.Models;
using AMS.Storage.Models;
using AMS.Storage.Models.Storage.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {
        public virtual DbSet<TblCstStudent> TblCstStudent { get; set; }
        public virtual DbSet<TblCstSchoolStudent> TblCstSchoolStudent { get; set; }
        public virtual DbSet<TblCstStudentContact> TblCstStudentContact { get; set; }
        public virtual DbQuery<ViewStudent> ViewStudent { get; set; }


        public void ApplyConfigurationCst(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblCstStudentMap());
            modelBuilder.ApplyConfiguration(new TblCstSchoolStudentMap());
            modelBuilder.ApplyConfiguration(new TblCstStudentContactMap());
        }
    }
}
