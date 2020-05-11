/*此代码由生成工具字段生成，生成时间2018/11/5 19:07:51 */
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblDatCost仓储
    /// </summary>
    public class TblDatCostRepository : BaseRepository<TblDatCost>
    {
        public TblDatCostRepository(DbContext context) : base(context)
        {

        }


        public TblDatCostRepository()
        {

        }

        public List<TblDatCost> GetCosts(IEnumerable<long> costId)
        {
            return base.LoadList(x => costId.Contains(x.CostId));
        }
    }
}
