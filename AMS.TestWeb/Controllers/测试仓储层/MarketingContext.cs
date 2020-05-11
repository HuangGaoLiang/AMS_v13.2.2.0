
using Jerrisoft.Platform.Public.Common.Extensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jerrisoft.Platform.Controllers
{
    public class BaseRepository<TEntity> : EFRepository<MarketingContext, TEntity> where TEntity : class
    {
        public MarketingContext CurrentContext
        {
            get { return (MarketingContext)base.Context; }
        }

        public BaseRepository()//:base(new MarketingContext())
        {
        }
    }

    public partial class MarketingContext : DbContext
    {
        public MarketingContext(DbContextOptions<MarketingContext> opt) : base(opt)
        {

        }


        public MarketingContext()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = AppConfigurtaionServices.GetAppSettings<AppConfigurtaionModel>("ConnectionStrings",
        $"appsettings.json").TestDataBaseContext;
                optionsBuilder.UseSqlServer(config);

            }
        }

        public class AppConfigurtaionModel
        { 

            public string TestDataBaseContext { get; set; }
            public string JwtKey { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfigurationActivity(modelBuilder);
        }
    }
}
