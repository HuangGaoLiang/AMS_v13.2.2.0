using AMS.Storage.Models;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退费订单交易
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-11-5</para>
    /// </summary>
    public abstract class RefundOrderTrade
    {
        /// <summary>
        /// 描述：退费订单信息
        /// </summary>
        protected readonly TblOdrRefundOrder RefundOrder;
        /// <summary>
        /// 描述：退费订单交易类实例化
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-11-5</para>
        /// </summary>
        /// <param name="refundOrder"></param>
        protected RefundOrderTrade(TblOdrRefundOrder refundOrder)
        {
            RefundOrder = refundOrder;
        }
    }
}

