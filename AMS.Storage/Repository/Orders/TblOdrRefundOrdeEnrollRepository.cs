/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System.Collections.Generic;
using AMS.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// TblOdrRefundOrdeEnroll仓储
    /// </summary>
    public class TblOdrRefundOrdeEnrollRepository : BaseRepository<TblOdrRefundOrdeEnroll>
    {
        public TblOdrRefundOrdeEnrollRepository()
        {
        }

        public TblOdrRefundOrdeEnrollRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据退费订单Id获取退费订单课程明细信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-16</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns>订单课程明细列表</returns>
        public List<TblOdrRefundOrdeEnroll> GetRefundOrderByOrderEnroll(long refundOrderId)
        {
            return base.LoadList(x => x.RefundOrderId == refundOrderId);
        }

        /// <summary>
        /// 根据退费订单Id获取退费订单课程明细信息
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-16</para>
        /// </summary>
        /// <param name="refundOrderId">一组退费订单Id</param>
        /// <returns>订单课程明细列表</returns>
        public List<TblOdrRefundOrdeEnroll> GetRefundOrderByOrderEnroll(List<long> refundOrderId)
        {
            return base.LoadList(x => refundOrderId.Contains(x.RefundOrderId));
        }
    }
}
