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
    /// 描述：TblOdrRefundOrderCost仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间:2018-11-9</para>
    /// </summary>
    public class TblOdrRefundOrderCostRepository : BaseRepository<TblOdrRefundOrderCost>
    {
        /// <summary>
        /// 描述：实例化一个TblOdrRefundOrderCost仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间:2018-11-9</para>
        /// </summary>
        public TblOdrRefundOrderCostRepository()
        {
        }

        /// <summary>
        /// 描述：实例化一个TblOdrRefundOrderCost仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间:2018-11-9</para>
        /// </summary>
        /// <param name="dbContext"></param>
        public TblOdrRefundOrderCostRepository(DbContext dbContext) : base(dbContext)
        {
        }


        /// <summary>
        /// 描述：根据退费订单Id获取其他费用集合
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间:2018-11-9</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns>其他费用信息集合</returns>
        public List<TblOdrRefundOrderCost> GetrefundOrderIdByCost(long refundOrderId)
        {
            return base.LoadList(x => x.RefundOrderId == refundOrderId);
        }

        /// <summary>
        /// 根据退费订单Id清空扣款项
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        public void ClearRefundOrderCostByRefundOrderId(long refundOrderId)
        {
            base.Delete(x => x.RefundOrderId == refundOrderId);
        }
    }
}
