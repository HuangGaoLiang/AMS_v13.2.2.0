using AMS.Core;
using AMS.Dto;
using Jerrisoft.Platform.Public.PageExtensions;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 与课程相关的所有费用订单
    /// </summary>
    public abstract class BaseOrderService : BService
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        protected virtual string OrderNoTag { get; }  //订单类型

        /// <summary>
        /// 产生订单号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        protected virtual string CreateOrderNo(long orderId)
        {
            return $"{OrderNoTag}{orderId}";
        }

        /// <summary>
        /// 获取订单详情
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2018-11-11</para>
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public abstract IOrderDetailResponse GetOrderDetail(long orderId,string companyId);

        /// <summary>
        /// 订单分页列表查询
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2019-03-05</para>
        /// </summary>
        /// <param name="searcher">查询条件</param>
        /// <returns></returns>
        public abstract LeaveSchoolOrderListResponse GetOrderList(IOrderListSearchRequest searcher);


        /// <summary>
        /// 取消/作废/撤销
        /// <para>作    者：蔡亚康</para>
        /// <para>创建时间：2018-11-11</para>
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="cancelUserId">取消操作人</param>
        /// <param name="cancelUserName">作废人姓名</param>
        /// <param name="cancelRemark">取消备注</param>
        public abstract Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark);

    }
}
