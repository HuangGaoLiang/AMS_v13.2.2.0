using AMS.Dto;
using AMS.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AMS.Storage.Repository.Orders
{
    /// <summary>
    /// 描述：休学订单自定义仓储
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ViewLeaveSchoolOrderRepository : BaseRepository<ViewLeaveSchoolOrder>
    {
        /// <summary>
        /// 描述：实例化一个休学订单自定义仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间:2019-3-6</para>
        /// </summary>
        public ViewLeaveSchoolOrderRepository()
        {
        }

        /// <summary>
        /// 描述：实例化一个休学订单仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间:2019-3-6</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewLeaveSchoolOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 描述：基础查询，获取校区的休学信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>休学列表</returns>
        private IQueryable<ViewLeaveSchoolOrder> QueryLeaveSchoolList(string schoolId, LeaveSchoolOrderListSearchRequest searcher)
        {
            var querySql = $@"select a.RefundOrderId, 
                                     b.StudentId,
                                     StudentNo,
                                     StudentName,
                                     b.CreateTime,
                                     LeaveTime,
                                     ResumeTime,
                                     TotalRefundLessonCount,
                                     b.Amount,
                                     Reason 
                             from  (select * from [TblOdrLeaveSchoolOrder] where SchoolId=@SchoolId ) as a
                                    left join TblOdrRefundOrder as b on a.RefundOrderId = b.RefundOrderId
                                    left join[TblCstStudent] as c on b.StudentId = c.StudentId";

            var leaveSchoolList = base.CurrentContext.ViewLeaveSchoolOrder.FromSql(querySql, new object[] {
                new SqlParameter("@SchoolId",schoolId.Trim()),
            });
            var leaveSchoolQuery = leaveSchoolList
                                    .WhereIf(searcher.CreateStart.HasValue, x => x.CreateTime >= searcher.CreateStart)                          //提交时间 开始
                                    .WhereIf(searcher.CreateEnd.HasValue, x => x.CreateTime <= searcher.CreateEnd.Value.AddDays(1))             //提交时间 结束
                                    .WhereIf(!string.IsNullOrWhiteSpace(searcher.StudentName), x => x.StudentName.Contains(searcher.StudentName))  //学生姓名
                                    ;
            return leaveSchoolQuery;
        }

        /// <summary>
        /// 描述：获取休学订单分页列表
        /// <para>作者;瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>休学订单列表</returns>
        public PageResult<ViewLeaveSchoolOrder> GetLeaveSchoolOrderList(string schoolId, LeaveSchoolOrderListSearchRequest searcher)
        {
            var leaveSchoolQuery = this.QueryLeaveSchoolList(schoolId, searcher)
                                    //.WhereIf(searcher.CreateStart.HasValue, x => x.CreateTime >= searcher.CreateStart)                          //提交时间 开始
                                    //.WhereIf(searcher.CreateEnd.HasValue, x => x.CreateTime <= searcher.CreateEnd.Value.AddDays(1))             //提交时间 结束
                                    //.WhereIf(!string.IsNullOrWhiteSpace(searcher.StudentName),x=>x.StudentName.Contains(searcher.StudentName))  //学生姓名
                                    //.OrderByDescending(x => x.CreateTime)
                                    .OrderByDescending(x => x.CreateTime)
                                    .ToPagerSource(searcher.PageIndex, searcher.PageSize);

            return leaveSchoolQuery;
        }
        /// <summary>
        /// 描述：获取休学人数
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-21</para>
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns></returns>
        public int QueryLeaveSchoolCount(string schoolId, LeaveSchoolOrderListSearchRequest searcher)
        {
            var leaveSchoolCount = this.QueryLeaveSchoolList(schoolId, searcher).Count();
            return leaveSchoolCount;
        }

        /// <summary>
        /// 描述：获取休学的退费总金额
        /// <para>作者：瞿琦</para>
        ///  <para>创建时间：2019-3-21</para>
        /// </summary>
        /// <returns></returns>
        public decimal QueryLeaveSchoolAmount(string schoolId, LeaveSchoolOrderListSearchRequest searcher)
        {
            var leaveSchoolAmount = this.QueryLeaveSchoolList(schoolId, searcher).Sum(x => x.Amount);
            return leaveSchoolAmount;
        }
    }
}
