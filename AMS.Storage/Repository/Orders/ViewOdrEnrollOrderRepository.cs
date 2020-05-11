using AMS.Dto;
using AMS.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;

namespace AMS.Storage.Repository.Orders
{
    /// <summary>
    /// 描    述: ViewOdrEnrollOrder仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class ViewOdrEnrollOrderRepository : BaseRepository<ViewOdrEnrollOrder>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ViewOdrEnrollOrderRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewOdrEnrollOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据条件获取报班列表
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-23</para>
        /// </summary>
        /// <param name="searcher">查询条件</param>
        /// <returns>返回分页列表</returns>
        public PageResult<ViewOdrEnrollOrder> GetOrderList(EnrollOrderListSearchRequest searcher)
        {
            var querySql = $@"SELECT o.EnrollOrderId,
                                       o.OrderNo,
                                       o.OrderNewType,
                                       o.PayType as PayTypeId,
                                       o.CreateName,
                                       o.PayAmount,
                                       o.OrderStatus as OrderStatusId,
                                       s.StudentId,
                                       s.StudentName,
                                       s.ContactPersonMobile,
                                       o.SchoolId,
                                       o.UseBalance,
                                       o.TotalTradeAmount,
                                       o.CreateTime
                                FROM dbo.TblOdrEnrollOrder o
                                    LEFT JOIN dbo.TblCstStudent s
                                        ON s.StudentId = o.StudentId
                                WHERE o.SchoolId = @SchoolId";
            var orderListQuery = base.CurrentContext.ViewOdrEnrollOrder.FromSql(querySql, new object[]
            {
                new SqlParameter("@SchoolId", searcher.SchoolId),
            });
            var query = orderListQuery
                .WhereIf(searcher.OrderStatus != null, x => x.OrderStatusId == (int)searcher.OrderStatus)
                .WhereIf(searcher.PayType != null && searcher.PayType.Value != PayType.UseBalance, x => x.PayTypeId == (int)searcher.PayType)
                .WhereIf(searcher.PayType != null && searcher.PayType.Value == PayType.UseBalance, x => x.UseBalance > 0)
                .WhereIf(searcher.OrderNewType != null, x => x.OrderNewType == (int)searcher.OrderNewType)
                .WhereIf(searcher.StartTime != null, x => x.CreateTime >= searcher.StartTime)
                .WhereIf(searcher.EndtTime != null, x => x.CreateTime.AddDays(-1) < searcher.EndtTime)
                .WhereIf(!string.IsNullOrWhiteSpace(searcher.StudentInfo), x => x.StudentName.Contains(searcher.StudentInfo) || x.ContactPersonMobile.Contains(searcher.StudentInfo))
                .WhereIf(!string.IsNullOrWhiteSpace(searcher.Cashier), x => x.CreateName.Contains(searcher.Cashier))
                .OrderByDescending(x => x.CreateTime)
                .ToPagerSource(searcher.PageIndex, searcher.PageSize);
            return query;
        }
    }
}
