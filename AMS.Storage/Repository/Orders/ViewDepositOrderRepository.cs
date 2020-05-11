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
    /// 描    述: ViewDepositOrder仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class ViewDepositOrderRepository : BaseRepository<ViewDepositOrder>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ViewDepositOrderRepository()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewDepositOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据查询条件获取获取订金列表
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-12-23</para>
        /// </summary>
        /// <param name="searcher">查询条件</param>
        /// <returns>返回分页数据</returns>
        public PageResult<ViewDepositOrder> GetOrderByWhere(DepositOrderListSearchRequest searcher)
        {
            var querySql = $@"SELECT d.DepositOrderId,
                                   d.OrderNo,
                                   d.StudentId,
                                   s.StudentName,
                                   s.ContactPersonMobile AS Mobile,
                                   d.Amount,
                                   d.PayType,
                                   d.PayeeId,
                                   d.Payee,
                                   d.UsesType,
                                   d.OrderStatus,
                                   d.PayDate,
                                   d.CreateTime
                            FROM TblOdrDepositOrder d
                                LEFT JOIN dbo.TblCstStudent s
                                    ON s.StudentId = d.StudentId
                            WHERE d.SchoolId =@SchoolId";
            var orderListQuery = base.CurrentContext.ViewDepositOrder.FromSql(querySql, new object[]
            {
                new SqlParameter("@SchoolId", searcher.SchoolId),
            });
            var query = orderListQuery
                .WhereIf(searcher.OrderStatus != null, x => x.OrderStatus == (int)searcher.OrderStatus)
                .WhereIf(searcher.PayType != null, x => x.PayType == (int)searcher.PayType)
                .WhereIf(searcher.UsesType != null, x => x.UsesType == (int)searcher.UsesType)
                .WhereIf(searcher.StartPayDate != null, x => x.CreateTime >= searcher.StartPayDate)
                .WhereIf(searcher.EndPayDate != null, x => x.CreateTime.AddDays(-1) < searcher.EndPayDate)
                .WhereIf(!string.IsNullOrWhiteSpace(searcher.Payee), x => x.Payee.Contains(searcher.Payee))
                .WhereIf(!string.IsNullOrWhiteSpace(searcher.StudentInfo), x => x.StudentName.Contains(searcher.StudentInfo) || x.Mobile.Contains(searcher.StudentInfo))
                .OrderByDescending(x => x.CreateTime)
                .ToPagerSource(searcher.PageIndex, searcher.PageSize);
            return query;
        }
    }
}
