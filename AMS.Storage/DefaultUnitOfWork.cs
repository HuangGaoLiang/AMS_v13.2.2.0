using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace AMS.Storage
{
    /// <summary>
    /// 默认事务单元
    /// </summary>
    public abstract class DefaultUnitOfWork<TDbContext> : UnitOfWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected DefaultUnitOfWork(TDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            DbContext.Database.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public virtual void CommitTransaction()
        {
            IDbContextTransaction transaction = DbContext.Database.CurrentTransaction;
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public virtual void RollbackTransaction()
        {
            IDbContextTransaction transaction = DbContext.Database.CurrentTransaction;
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }
    }
}
