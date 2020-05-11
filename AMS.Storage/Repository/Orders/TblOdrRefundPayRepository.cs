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
    /// 描述：TblOdrRefundPay仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-9</para>
    /// </summary>
    public class TblOdrRefundPayRepository : BaseRepository<TblOdrRefundPay>
    {
        /// <summary>
        /// 描述：实例化一个TblOdrRefundPay仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        public TblOdrRefundPayRepository()
        {
        }

        /// <summary>
        /// 描述：实例化一个TblOdrRefundPay仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblOdrRefundPayRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：根据退费订单主键获取支付信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单表Id</param>
        /// <returns>支付信息</returns>
        public TblOdrRefundPay GetRefundOrderIdByRefundPay(long refundOrderId)
        {
            return base.Load(x => x.RefundOrderId == refundOrderId);
        }
    }
}
