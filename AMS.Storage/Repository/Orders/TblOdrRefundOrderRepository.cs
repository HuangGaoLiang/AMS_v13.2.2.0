/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Dto;
using AMS.Storage.Context;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblOdrRefundOrder仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-9</para>
    /// </summary>
    public class TblOdrRefundOrderRepository : BaseRepository<TblOdrRefundOrder>
    {
        /// <summary>
        /// 描述：实例化一个TblOdrRefundOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        public TblOdrRefundOrderRepository()
        {
        }

        /// <summary>
        /// 描述：实例化一个TblOdrRefundOrder仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="context"></param>
        public TblOdrRefundOrderRepository(AMSContext context) : base(context)
        {
        }
       

        /// <summary>
        /// 描述：根据主键Id获取退费订单信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns></returns>
        public TblOdrRefundOrder GetRefundOrderIdByRefundOrder(long refundOrderId)
        {
            return base.Load(x => x.RefundOrderId == refundOrderId);
        }

        /// <summary>
        /// 获取转校订单信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">查询参数</param>
        /// <param name="studentIds">一组学生Id</param>
        /// <returns>转校订单信息列表</returns>
        public PageResult<TblOdrRefundOrder> GetPageResult(string schoolId, ChangeSchoolOrderListSearchRequest request, List<long> studentIds)
        {
            var queryable = base.LoadQueryable().Where(x => x.SchoolId == schoolId);

            if (request.OrderStatus.HasValue)
            {
                int orderStatus = (int)request.OrderStatus.Value;
                queryable = queryable.Where(x => x.OrderStatus == orderStatus);
            }

            if (request.STime.HasValue)
            {
                queryable = queryable.Where(x => x.CreateTime >= request.STime.Value);
            }

            if (request.ETime.HasValue)
            {
                request.ETime = request.ETime.Value.AddDays(1);
                queryable = queryable.Where(x => x.CreateTime <= request.ETime.Value);
            }

            if (studentIds.Any())
            {
                queryable = queryable.Where(x => studentIds.Contains(x.StudentId));
            }

            queryable = queryable.OrderByDescending(x => x.CreateTime);

            return queryable.ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 描述：根据校区和学生Id获取退费订单休息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间： 2018-12-5</para>
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public TblOdrRefundOrder GetRefundOrderBySchOrStu(string schoolId, long studentId)
        {
            var result = base.LoadList(x => x.SchoolId.Trim() == schoolId && x.StudentId == studentId && x.OrderType == (int)OrderTradeType.LeaveSchoolOrder).OrderByDescending(x => x.CreateTime).FirstOrDefault();
            return result;
        }

    }
}
