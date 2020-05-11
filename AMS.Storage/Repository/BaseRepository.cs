/*此代码由生成工具字段生成，生成时间2018/9/7 12:42:58 */
using Jerrisoft.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using AMS.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    public class BaseRepository<TEntity> : EFRepository<AMSContext, TEntity> where TEntity : class
    {
        public AMSContext CurrentContext
        {
            get { return (AMSContext)base.Context; }
        }

        public BaseRepository() : base(new AMSContext())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public BaseRepository(DbContext dbContext) : base(dbContext)
        { }


        /// <summary>
        /// 将数组转换成in语句，仅适用于数字类型的，
        /// 如果字符串类型一定要用parameters防止注入
        /// </summary>
        /// <param name="values">一组要用in的数组值</param>
        /// <returns></returns>
        protected string GetSqlIn(List<long> values)
        {
            return string.Join(",",values);
        }
        /// <summary>
        /// 将数组转换成in语句，仅适用于数字类型的，
        /// 如果字符串类型一定要用parameters防止注入
        /// </summary>
        /// <param name="values">一组要用in的数组值</param>
        /// <returns></returns>
        protected string GetSqlIn(List<int> values)
        {
            return string.Join(",", values);
        }

    }
}
