/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblOdrLeaveSchoolOrder仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-15</para>
    /// </summary>
    public class TblOdrLeaveSchoolOrderRepository : BaseRepository<TblOdrLeaveSchoolOrder>
    {
        /// <summary>
        /// 描述：实例化一个TblOdrLeaveSchoolOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        public TblOdrLeaveSchoolOrderRepository()
        {
        }
        /// <summary>
        /// 描述：实例化一个TblOdrLeaveSchoolOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="dbContext"></param>
        public TblOdrLeaveSchoolOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：根据退费订单Id主键获取休学信息
        /// <para>作   者;瞿琦</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns></returns>
        public TblOdrLeaveSchoolOrder GetLeaveSchoolIdByOrder(long refundOrderId)
        {
            return Load(x=>x.RefundOrderId== refundOrderId);
        }
    }
}
