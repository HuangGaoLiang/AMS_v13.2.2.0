/*此代码由生成工具字段生成，生成时间2018/11/14 10:48:12 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using AMS.Dto;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatOperationLog仓储
    /// </summary>
    public class TblDatOperationLogRepository : BaseRepository<TblDatOperationLog>
    {
        public TblDatOperationLogRepository()
        {
        }

        public TblDatOperationLogRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取业务记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="businessId">业务Id</param>
        /// <returns>业务操作记录列表</returns>
        public List<TblDatOperationLog> GetList(long businessId)
        {
            return base.LoadList(x => x.BusinessId == businessId).OrderBy(x => x.CreateTime).ToList();
        }
    }
}
