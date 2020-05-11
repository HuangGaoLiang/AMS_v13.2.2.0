/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblOdrLeaveClassOrder仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-5</para>
    /// </summary>
    public class TblOdrLeaveClassOrderRepository : BaseRepository<TblOdrLeaveClassOrder>
    {
        /// <summary>
        /// 描述：实例化一个TblOdrLeaveClassOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        public TblOdrLeaveClassOrderRepository()
        {
        }
        /// <summary>
        /// 描述：实例化一个TblOdrLeaveClassOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        /// <param name="dbContext"></param>
        public TblOdrLeaveClassOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：根据退费订单主表Id获取退班信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-5</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns></returns>
        public TblOdrLeaveClassOrder GetRefundOrderByLeaveClassOrder(long refundOrderId)
        {
            return base.Load(x=>x.RefundOrderId== refundOrderId);
        }
    }
}
