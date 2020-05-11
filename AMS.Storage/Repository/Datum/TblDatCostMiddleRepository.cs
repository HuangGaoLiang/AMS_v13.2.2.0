/*此代码由生成工具字段生成，生成时间2018/11/5 19:07:51 */
using System.Collections.Generic;
using System.Linq;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatCostMiddle仓储
    /// </summary>
    public class TblDatCostMiddleRepository : BaseRepository<TblDatCostMiddle>
    {
        public TblDatCostMiddleRepository(DbContext context) : base(context)
        {

        }

        public TblDatCostMiddleRepository()
        {

        }

        /// <summary>
        /// 根据业务类型获取其分配的费用
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public List<TblDatCostMiddle> GetByCostType(string typeCode)
        {
            return this.LoadQueryable(t => t.TypeCode == typeCode, false)
                        .ToList();
        }
    }
}
