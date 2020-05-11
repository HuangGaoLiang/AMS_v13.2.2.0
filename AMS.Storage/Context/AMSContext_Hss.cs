using AMS.Models;
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using AMS.Storage.Models.Storage.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    /// <summary>
    /// 家校互联模块实体映射
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public partial class AMSContext
    {
        public virtual DbSet<TblHssPassport> TblHssPassport { get; set; }


        /// <summary>
        /// 加载实体映射
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void ApplyConfigurationHss(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblHssPassportMap());
        }
    }
}
