using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.API.Controllers.Internal.Orders
{
    /// <summary>
    /// 描    述：余额退费
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-08</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/BalanceRefund")]
    [ApiController]
    public class BalanceRefundController : BsnoController
    {
        /// <summary>
        /// 余额退费办理
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">余额退费请求对象</param>
        [HttpPost]
        public void Post(long studentId, BalanceRefundOrderRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            request.CreateTime = DateTime.Now;
            new BalanceRefundOrderServices(base.SchoolId, studentId).Transact(request,base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 办理前获取学生的余额信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">余额退费详情请求对象</param>
        /// <returns>余额退费详情</returns>
        [HttpGet]
        public BalanceRefundOrderTransactDetailResponse Get(long studentId, [FromQuery] BalanceRefundTransactDetailRequest request)
        {
            return (BalanceRefundOrderTransactDetailResponse)new BalanceRefundOrderServices(base.SchoolId, studentId).GetTransactDetail(request, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 获取余额退费列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="request">余额退费查询条件</param>
        /// <returns></returns>
        [HttpGet, Route("GetBalanceRefundList")]
        public PageResult<BalanceRefundOrderListResponse> GetBalanceRefundList([FromQuery]BalanceRefundListSearchRequest request)
        {
            request.SchoolId = base.SchoolId;
            return BalanceRefundOrderServices.GetBalanceRefundList(request);
        }

        /// <summary>
        /// 获取余额退费订单详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="studenId">学生Id</param>
        /// <param name="orderId">余额退费订单Id</param>
        /// <returns>余额退费详细信息（包含附件）</returns>
        [HttpGet, Route("GetBalanceRefundDetail")]
        public BalanceRefundOrderDetailResponse GetBalanceRefundDetail(long studenId, long orderId)
        {
            return new BalanceRefundOrderServices(base.SchoolId, studenId).GetOrderDetail(orderId, base.CurrentUser.CompanyId) as BalanceRefundOrderDetailResponse;
        }

        /// <summary>
        /// 获取余额退费订单的打印收据详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="studenId">学生Id</param>
        /// <param name="orderId">余额退费订单Id</param>
        /// <returns>余额退费打印信息</returns>
        [HttpGet, Route("GetPrintReceiptDetail")]
        public BalanceRefundReceiptDetailResponse GetPrintReceiptDetail(long studenId, long orderId)
        {
            return new BalanceRefundOrderServices(base.SchoolId, studenId).GetPrintReceiptDetail(orderId);
        }
    }
}
