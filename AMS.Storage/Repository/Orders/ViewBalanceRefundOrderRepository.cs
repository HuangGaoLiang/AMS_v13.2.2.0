using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描    述：余额退费业务逻辑仓储
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class ViewBalanceRefundOrderRepository : BaseRepository<ViewBalanceRefundOrder>
    {
        /// <summary>
        /// 余额退费业务逻辑仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        public ViewBalanceRefundOrderRepository()
        {
        }

        /// <summary>
        /// 余额退费业务逻辑仓储实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        public ViewBalanceRefundOrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 获取余额退费明细列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="request">余额退费请求对象</param>
        /// <returns>余额退费信息列表</returns>
        public PageResult<ViewBalanceRefundOrder> GetBalanceRefundList(BalanceRefundListSearchRequest request)
        {
            var result = from a in CurrentContext.TblOdrRefundOrder
                         join b in CurrentContext.TblOdrRefundPay on a.RefundOrderId equals b.RefundOrderId
                         join c in CurrentContext.TblCstStudent on a.StudentId equals c.StudentId
                         let mobile = (c.ContactPersonMobile.IndexOf(",") >= 0 ? c.ContactPersonMobile.Remove(c.ContactPersonMobile.IndexOf(",")) : c.ContactPersonMobile)
                         where a.SchoolId == request.SchoolId && a.OrderType == (int)OrderTradeType.BalanceOrder
                         && (!request.RefundBeginDate.HasValue || a.CreateTime.Date >= request.RefundBeginDate.Value.Date)
                         && (!request.RefundEndDate.HasValue || a.CreateTime.Date <= request.RefundEndDate.Value.Date)
                         && (string.IsNullOrEmpty(request.Keyword) || (c.StudentName.Contains(request.Keyword) || (mobile.Contains(request.Keyword))))
                         select new ViewBalanceRefundOrder
                         {
                             OrderId = a.RefundOrderId,
                             OrderNo = a.OrderNo,
                             StudentId = a.StudentId,
                             StudentNo = c.StudentNo,
                             StudentName = c.StudentName,
                             RefundType = b.RefundType,
                             Amount = a.Amount,
                             TotalDeductAmount = a.TotalDeductAmount,
                             RealRefundAmount = (a.Amount - a.TotalDeductAmount),
                             GuardianMobile = mobile,
                             BankCardNo = b.BankCardNo,
                             BankName = b.BankName,
                             BankUserName = b.BankUserName,
                             Remark = b.Remark,
                             RefundDate = a.CreateTime,
                             CreatorName = a.CreatorName
                         };
            var resultList = result.ToList();
            return result.OrderByDescending(a => a.RefundDate).ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 获取余额退费明细详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="orderId">余额退费订单Id</param>
        /// <returns>余额退费明细详情</returns>
        public ViewBalanceRefundOrder GetBalanceRefundByOrderId(long orderId)
        {
            var result = from a in CurrentContext.TblOdrRefundOrder
                         join b in CurrentContext.TblOdrRefundPay on a.RefundOrderId equals b.RefundOrderId
                         join c in CurrentContext.TblCstStudent on a.StudentId equals c.StudentId into c_left
                         from c in c_left.DefaultIfEmpty()
                         where a.RefundOrderId == orderId
                         select new ViewBalanceRefundOrder
                         {
                             OrderId = a.RefundOrderId,
                             SchoolId = a.SchoolId,
                             OrderNo = a.OrderNo,
                             StudentId = a.StudentId,
                             StudentNo = c.StudentNo,
                             StudentName = c.StudentName,
                             RefundType = b.RefundType,
                             Amount = a.Amount,
                             TotalDeductAmount = a.TotalDeductAmount,
                             RealRefundAmount = (a.Amount - a.TotalDeductAmount),
                             GuardianMobile = c.ContactPersonMobile,
                             BankCardNo = b.BankCardNo,
                             BankName = b.BankName,
                             BankUserName = b.BankUserName,
                             Remark = b.Remark,
                             RefundDate = a.CreateTime,
                             CreatorName = a.CreatorName
                         };
            return result.FirstOrDefault();
        }
    }
}
