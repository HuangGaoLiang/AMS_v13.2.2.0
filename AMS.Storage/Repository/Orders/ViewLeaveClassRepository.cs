using System.Data.SqlClient;
using System.Linq;
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository.Orders
{
    /// <summary>
    /// 描述：报班退费明细
    /// <para>作    者:瞿琦</para>
    /// <para>创建时间:2018-11-12</para>
    /// </summary>
    public class ViewLeaveClassDetailRepository : BaseRepository<ViewLeaveClassDetail>
    {
        /// <summary>
        /// 描述：实例化一个报班退费明细仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间:2018-11-12</para>
        /// </summary>
        public ViewLeaveClassDetailRepository()
        {
        }
        /// <summary>
        /// 描述：实例化一个报班退费明细仓储对象
        /// <para>作    者:瞿琦</para>
        /// <para>创建时间:2018-11-12</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public ViewLeaveClassDetailRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：获取报班退费列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>报班退费明细</returns>
        public PageResult<ViewLeaveClassDetail> GetLeaveClassByRefundOrder(LeaveClassOrderListSearchRequest searcher)
        {
            var querySql = $@"select a.RefundOrderId,
                                     a.SchoolId,
                                     a.StudentId,
                                     a.CreateTime as OrderRefundTime,
                                     a.OrderNo,
                                     b.StudentNo,
                                     b.StudentName,
                                     b.LinkMobile,
                                     a.Amount,
                                     c.RefundType,
                                     a.OrderStatus,
                                     b.ContactPersonMobile 
                              from [dbo].[TblOdrRefundOrder] a
                                     left join TblCstStudent b on a.StudentId = b.StudentId
                                     left join TblOdrRefundPay c on a.RefundOrderId = c.RefundOrderId 
                              where  a.OrderType=@OrderType          
                                     and a.OrderStatus!=@OrderStatus1    
                                     and a.OrderStatus!=@OrderStatus2
                                     ";             //order 作     者 a.CreateTime desc 
            var refundOrderListQuery = base.CurrentContext.ViewLeaveClassDetail.FromSql(querySql, new object[]
             {
                new SqlParameter("@OrderType",(int)OrderTradeType.LeaveClassOrder),
                new SqlParameter("@OrderStatus1",(int)OrderStatus.Cancel),
                new SqlParameter("@OrderStatus2",(int)OrderStatus.Finish),
             });
            var query = refundOrderListQuery.WhereIf(!string.IsNullOrWhiteSpace(searcher.SchoolId), x => x.SchoolId.Trim() == searcher.SchoolId.Trim())    //校区
                                .WhereIf(searcher.OrderStatus != null, x => x.OrderStatus == (int)searcher.OrderStatus)          //订单状态
                                .WhereIf(searcher.RefundBeginDate != null, x => x.OrderRefundTime >= searcher.RefundBeginDate)   //退费日期（开始）
                                .WhereIf(searcher.RefundEndDate != null, x => x.OrderRefundTime <= searcher.RefundEndDate.Value.AddDays(1))  //退费日期（结束）
                                .WhereIf(!string.IsNullOrWhiteSpace(searcher.KeyWord), x => x.StudentName.Contains(searcher.KeyWord) || x.LinkMobile.Contains(searcher.KeyWord))//关键字（学生姓名/手机号）
                                .OrderByDescending(x => x.OrderRefundTime)
                                .ToPagerSource(searcher.PageIndex, searcher.PageSize);
            return query;
        }
    }
}
