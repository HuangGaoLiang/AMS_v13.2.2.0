/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblOdrCampOrder仓储
    /// </summary>
    public class TblOdrCampOrderRepository : BaseRepository<TblOdrCampOrder>
    {
        public TblOdrCampOrderRepository()
        {
        }

        public TblOdrCampOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
