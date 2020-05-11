using System.Data.SqlClient;
using System.Linq;
using AMS.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository.Orders
{
    /// <summary>
    /// 描    述: ViewOrderObsoleteMonth仓储
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class ViewOrderObsoleteMonthResponse : BaseRepository<ViewOrderObsoleteMonth>
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ViewOrderObsoleteMonthResponse()
        {
        }

        /// <summary>
        /// 带上下文的构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewOrderObsoleteMonthResponse(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取报班列表
        /// <para>作    者: Huang GaoLiang</para>
        /// <para>创建时间: 2018-11-23</para>
        /// </summary>
        /// <param name="stuName">学生姓名</param>
        /// <param name="schoolId">校区编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回报班分页数据</returns>
        public PageResult<ViewOrderObsoleteMonth> GetEnrollOrderPageList(string stuName, string schoolId, int pageIndex, int pageSize)
        {
            var querySql = $@"SELECT EnrollOrderId,
                                   SchoolId,
                                   e.CreateTime,
                                   OrderNo,
                                   e.StudentId,
                                   s.StudentName,
                                   s.ContactPersonMobile,
                                   CreateId as PayeeId,
                                   CreateName AS Payee,
                                   e.UseBalance,
                                   e.TotalTradeAmount,
                                   e.PayAmount,
                                   e.PayType,
                                   e.OrderStatus
                            FROM dbo.TblOdrEnrollOrder e
                                LEFT JOIN dbo.TblCstStudent s
                                    ON s.StudentId = e.StudentId
                                WHERE e.OrderStatus=9 AND e.SchoolId = @SchoolId";
            var orderListQuery = base.CurrentContext.ViewOrderObsoleteMonth.FromSql(querySql, new object[]
            {
                new SqlParameter("@SchoolId", schoolId),
            });
            var query = orderListQuery
                .WhereIf(!string.IsNullOrWhiteSpace(stuName), m => m.StudentName.Contains(stuName))
                .OrderByDescending(m => m.CreateTime)
                .ToPagerSource(pageIndex, pageSize);
            return query;
        }
    }
}
