using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AMS.Dto;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：订单收款未交接的汇总仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class ViewFinOrderHandoverDetailSummaryRepository : BaseRepository<ViewFinOrderHandoverDetailSummary>
    {
        /// <summary>
        /// 订单收款未交接的汇总仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public ViewFinOrderHandoverDetailSummaryRepository()
        {
        }

        /// <summary>
        /// 订单收款未交接的汇总仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public ViewFinOrderHandoverDetailSummaryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取报班交接汇总数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>订单未交接汇总列表</returns>
        public List<OrderUnHandoverCountResponse> GetEnrollHandoverSummary(string schoolId)
        {
            var result = from a in CurrentContext.TblOdrEnrollOrder
                         join b in CurrentContext.TblFinOrderHandoverDetail on a.EnrollOrderId equals b.OrderId into ta
                         from c in ta.DefaultIfEmpty()
                         where a.OrderStatus == (int)OrderStatus.Paid
                         && (c.HandoverStatus == null || c.HandoverStatus != 2)
                         && a.SchoolId == schoolId
                         group a by new
                         {
                             a.SchoolId,
                             a.CreateName,
                             a.CreateId
                         } into g
                         select new OrderUnHandoverCountResponse
                         {
                             SchoolId = g.Key.SchoolId,
                             PersonalId = g.Key.CreateId,
                             PersonalName = g.Key.CreateName,
                             Amount = g.Sum(p => p.PayAmount),
                             UnHandoverNumber = g.Count()
                         };

            return result.ToList();
        }

        /// <summary>
        /// 获取定金交接汇总数据
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>订单未交接汇总列表</returns>
        public List<OrderUnHandoverCountResponse> GetDepositHandoverSummary(string schoolId)
        {
            var result = from a in CurrentContext.TblOdrDepositOrder
                         join b in CurrentContext.TblFinOrderHandoverDetail on a.DepositOrderId equals b.OrderId into ta
                         from c in ta.DefaultIfEmpty()
                         where a.OrderStatus == (int)OrderStatus.Paid
                         && (c.HandoverStatus == null || c.HandoverStatus != 2)
                         && a.SchoolId == schoolId
                         group a by new
                         {
                             a.SchoolId,
                             a.Payee,
                             a.PayeeId
                         } into g
                         select new OrderUnHandoverCountResponse
                         {
                             SchoolId = g.Key.SchoolId,
                             PersonalId = g.Key.PayeeId,
                             PersonalName = g.Key.Payee,
                             Amount = g.Sum(p => p.Amount),
                             UnHandoverNumber = g.Count()
                         };

            return result.ToList();
        }

        /// <summary>
        /// 根据请求参数获取对应的订单交接明细列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="request">订单交接明细请求对象</param>
        /// <returns>订单交接明细信息列表</returns>
        public PageResult<OrderHandoverDetailListResponse> GetDetailsByHandoverId(OrderHandoverDetailRequest request)
        {
            var result = from a in CurrentContext.TblFinOrderHandover
                         join b in CurrentContext.TblFinOrderHandoverDetail on a.OrderHandoverId equals b.OrderHandoverId
                         where a.OrderHandoverId == request.HandoverId
                         && a.SchoolId == request.SchoolId
                         select new OrderHandoverDetailListResponse
                         {
                             StudentId = b.StudentId,
                             OrderId = b.OrderId,
                             OrderNo = b.OrderNo,
                             OrderTradeType = b.OrderTradeType,
                             PayAmount = b.PayAmount,
                             HandoverDate = a.HandoverDate,
                             CreateTime = b.CreateTime,
                             PayDate = b.PayDate,
                             Remark = b.Remark
                         };
            return result.ToPagerSource(request.PageIndex, request.PageSize);
        }
    }
}
