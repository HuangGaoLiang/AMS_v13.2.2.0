/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblOdrRefundChangeSchoolOrder仓储
    /// </summary>
    public class TblOdrRefundChangeSchoolOrderRepository : BaseRepository<TblOdrRefundChangeSchoolOrder>
    {
        public TblOdrRefundChangeSchoolOrderRepository()
        {
        }

        public TblOdrRefundChangeSchoolOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public List<TblOdrRefundChangeSchoolOrder> GetByRefundOrderId(IEnumerable<long> refundOrderId)
        {
            return base.LoadList(x => refundOrderId.Contains(x.RefundOrderId));
        }
    }
}
