using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 转校订单仓储
    /// </summary>
    public class ViewChangeSchooolOrderRepository : BaseRepository<ViewChangeSchooolOrder>
    {
        public ViewChangeSchooolOrderRepository()
        {
        }

        public ViewChangeSchooolOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 转校订单查询器
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <returns>构建转校订单查询列表</returns>
        private IQueryable<ViewChangeSchooolOrder> GetTransferOrderQueryable()
        {
            var queryable = from a in base.CurrentContext.TblOdrRefundOrder.AsQueryable()
                            join b in base.CurrentContext.TblOdrRefundChangeSchoolOrder.AsQueryable()
                            on a.RefundOrderId equals b.RefundOrderId
                            select new ViewChangeSchooolOrder
                            {
                                Amount = a.Amount,
                                RefundOrderId = a.RefundOrderId,
                                CancelDate = a.CancelDate,
                                CancelRemark = a.CancelRemark,
                                CancelUserId = a.CancelUserId,
                                CreateTime = a.CreateTime,
                                CreatorId = a.CreatorId,
                                CreatorName = a.CreatorName,
                                InSchoolId = b.InSchoolId,
                                OrderNo = a.OrderNo,
                                OrderStatus = a.OrderStatus,
                                OrderType = a.OrderType,
                                OutDate = b.OutDate,
                                OutSchoolId = b.OutSchoolId,
                                ReceiptStatus = b.ReceiptStatus,
                                Remark = b.Remark,
                                SchoolId = a.SchoolId,
                                StudentId = a.StudentId,
                                TotalDeductAmount = a.TotalDeductAmount,
                                TotalRefundLessonCount = b.TotalRefundLessonCount,
                                TotalUseLessonCount = b.TotalUseLessonCount,
                                TransFromBalance = b.TransFromBalance
                            };

            return queryable;
        }

        /// <summary>
        /// 获取学生转校正常订单
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="schoolId">校区Id</param>
        /// <returns>转校订单列表</returns>
        public List<ViewChangeSchooolOrder> GetStudentChangeSchoolOrder(long studentId, string schoolId)
        {
            int cancel = (int)OrderStatus.Cancel; //要排除取消的订单
            int refuse = (int)OrderStatus.Refuse; //拒绝的订单

            var queryable = this.GetTransferOrderQueryable().Where(
                m => m.SchoolId == schoolId &&
                     m.StudentId == studentId &&
                     m.OrderStatus != cancel &&
                     m.OrderStatus != refuse)
                     .ToList();

            return queryable;
        }

        /// <summary>
        /// 获取转校订单
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrderId">订单Id</param>
        /// <returns>转校订单详情</returns>
        public ViewChangeSchooolOrder GetChangeSchooolOrder(long refundOrderId)
        {
            return this.GetTransferOrderQueryable().FirstOrDefault(x => x.RefundOrderId == refundOrderId);
        }

        /// <summary>
        ///  转出列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>转校订单信息列表</returns>
        public PageResult<ViewChangeSchooolOrder> GetChangeOutList(string schoolId, ChangeSchoolOrderListSearchRequest request, List<long> studentIds)
        {
            var queryable = this.GetTransferOrderQueryable().Where(x => x.OutSchoolId == schoolId);

            queryable = ViewChangeSchooolOrders(request, studentIds, queryable);

            return queryable.ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        ///  转入列表 
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <returns>转校订单信息列表</returns>
        public PageResult<ViewChangeSchooolOrder> GetChangeInList(string schoolId, ChangeSchoolOrderListSearchRequest request, List<long> studentIds)
        {
            int confirm = (int)OrderStatus.Confirm; //待接收
            int refuse = (int)OrderStatus.Refuse;   //已拒绝
            int paid = (int)OrderStatus.Paid;       //已接收

            var queryable = this.GetTransferOrderQueryable()
                .Where(x => x.InSchoolId == schoolId)
                .Where(x => x.OrderStatus == confirm || x.OrderStatus == refuse || x.OrderStatus == paid);

            queryable = ViewChangeSchooolOrders(request, studentIds, queryable);

            return queryable.ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 条件组合查询器
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-20</para>
        /// </summary>
        /// <param name="request">转校列表查询条件</param>
        /// <param name="studentIds">学生Id</param>
        /// <param name="queryable">订单列表</param>
        /// <returns>转校订单列表</returns>
        private static IQueryable<ViewChangeSchooolOrder> ViewChangeSchooolOrders(
            ChangeSchoolOrderListSearchRequest request,
            ICollection<long> studentIds,
            IQueryable<ViewChangeSchooolOrder> queryable)
        {
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

            if (!string.IsNullOrWhiteSpace(request.Key))
            {
                queryable = queryable.Where(x => studentIds.Contains(x.StudentId));
            }

            queryable = queryable.OrderByDescending(x => x.CreateTime);

            return queryable;
        }
    }
}
