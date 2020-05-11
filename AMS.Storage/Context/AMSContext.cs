using AMS.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace AMS.Storage.Context
{
    /// <summary>
    /// 表示一个AMS数据库上下文
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public partial class AMSContext : DbContext
    {

        /// <summary>
        /// 实例化一个数据库上下文
        /// </summary>
        /// <param name="options">配置选项</param>
        private AMSContext(DbContextOptions<AMSContext> options) : base(options)
        {
        }

        /// <summary>
        /// 实例化一个数据库上下文
        /// </summary>
        public AMSContext()
        {

        }


        /// <summary>
        /// 重写EF的连接配置
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="optionsBuilder">配置选项</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    string connStr = ClientConfigManager.AppsettingsConfig.ConnectionStrings.DefaultContext;
                    optionsBuilder.UseSqlServer(connStr);
                }
            }
        }

        /// <summary>
        /// 重写EF的实体映射创建
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfigurationDatum(modelBuilder);
            ApplyConfigurationTimetable(modelBuilder);
            ApplyConfigurationAudit(modelBuilder);
            ApplyConfigurationCash(modelBuilder);
            ApplyConfigurationDiscount(modelBuilder);
            ApplyConfigurationOrders(modelBuilder);
            ApplyConfigurationCst(modelBuilder);
            ApplyConfigurationReports(modelBuilder);
            ApplyConfigurationFinance(modelBuilder);
            ApplyConfigurationHss(modelBuilder);
        }
    }
}
