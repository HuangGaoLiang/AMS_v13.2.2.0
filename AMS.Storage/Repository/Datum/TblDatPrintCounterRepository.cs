/*此代码由生成工具字段生成，生成时间2018/10/29 16:39:30 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatPrintCounter仓储
    /// </summary>
    public class TblDatPrintCounterRepository : BaseRepository<TblDatPrintCounter>
    {
        public TblDatPrintCounterRepository()
        {
        }

        public TblDatPrintCounterRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取打印调用记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="year">年度</param>
        /// <param name="printBillType">打印类型</param>
        /// <returns>打印调用记录</returns>
        public TblDatPrintCounter Get(string schoolId, int year, byte printBillType)
        {
            return base.Load(x => x.SchoolId == schoolId && x.Year == year && x.PrintBillType == printBillType);
        }
    }
}
