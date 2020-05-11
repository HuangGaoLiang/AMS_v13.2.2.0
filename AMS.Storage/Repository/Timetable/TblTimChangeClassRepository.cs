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
    /// TblTimChangeClass仓储
    /// </summary>
    public class TblTimChangeClassRepository : BaseRepository<TblTimChangeClass>
    {
        public TblTimChangeClassRepository(DbContext context) : base(context)
        {

        }
        public TblTimChangeClassRepository()
        {

        }
    }
}
