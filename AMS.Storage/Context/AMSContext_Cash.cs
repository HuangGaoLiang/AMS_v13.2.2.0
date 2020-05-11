using AMS.Models;
using AMS.Storage.Mapping;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Context
{
    public partial class AMSContext
    {

        public virtual DbSet<TblCashOrderTrade> TblCashOrderTrade { get; set; }
        public virtual DbSet<TblCashWallet> TblCashWallet { get; set; }
        public virtual DbSet<TblCashWalletForzenDetail> TblCashWalletForzenDetail { get; set; }
        public virtual DbSet<TblCashWalletTrade> TblCashWalletTrade { get; set; }
        public void ApplyConfigurationCash(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TblCashWalletTradeMap());
            modelBuilder.ApplyConfiguration(new TblCashWalletForzenDetailMap());
            modelBuilder.ApplyConfiguration(new TblCashOrderTradeMap());
            modelBuilder.ApplyConfiguration(new TblCashWalletMap());
        }
    }
}
