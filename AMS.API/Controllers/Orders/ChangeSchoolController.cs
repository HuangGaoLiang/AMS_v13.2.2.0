using System;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 转校
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    [Route("api/AMS/ChangeSchool")]
    public class ChangeSchoolController : BsnoController
    {
        /// <summary>
        /// 本校转出办理-费用明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="time">转校时间</param>
        /// <returns>费用明细列表</returns>
        [HttpGet, Route("GetTransactDetail")]
        public ChangeSchoolOrderTransactDetailResponse GetTransactDetail(long studentId, DateTime time)
        {
            ChangeSchoolOrderTransactDetailRequest request = new ChangeSchoolOrderTransactDetailRequest
            {
                ChangeSchoolTime = time
            };

            var service = new ChangeSchoolOrderService(base.SchoolId, studentId);

            var result = service.GetTransactDetail(request, base.CurrentUser.CompanyId) as ChangeSchoolOrderTransactDetailResponse;

            return result;
        }

        /// <summary>
        /// 订单详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单详情</returns>
        [HttpGet, Route("GetChangeSchoolOrderDetail")]
        public ChangeSchoolOrderDetailResponse GetChangeSchoolOrderDetail(long studentId, long orderId)
        {
            var service = new ChangeSchoolOrderService(base.SchoolId, studentId);

            var result = service.GetOrderDetail(orderId, base.CurrentUser.CompanyId) as ChangeSchoolOrderDetailResponse;

            return result;
        }

        /// <summary>
        /// 本校转出-列表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns>本校转出列表</returns>
        [HttpGet, Route("GetChangeOutList")]
        public PageResult<ChangeSchoolOrderListResponse> GetChangeOutList([FromQuery]ChangeSchoolOrderListSearchRequest request)
        {
            return ChangeSchoolOrderService.GetChangeOutList(base.SchoolId, request);
        }

        /// <summary>
        /// 他校转入-列表数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">搜索条件</param>
        /// <returns>他校转入列表</returns>
        [HttpGet, Route("GetChangeInList")]
        public PageResult<ChangeSchoolOrderListResponse> GetChangeInList([FromQuery]ChangeSchoolOrderListSearchRequest request)
        {
            return ChangeSchoolOrderService.GetChangeInList(base.SchoolId, request);
        }

        /// <summary>
        /// 本校转出办理-提交给财务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">转校订单数据</param>
        [HttpPost, Route("Transact")]
        public void Transact(long studentId, [FromBody]ChangeSchoolOrderAddRequest request)
        {
            request.OperatorId = base.CurrentUser.UserId;
            request.OperatorName = base.CurrentUser.UserName;

            var service = new ChangeSchoolOrderService(base.SchoolId, studentId);
            service.Transact(request, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 本校转出-撤销
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="request">撤销原因</param>
        [HttpPost, Route("Cancel")]
        public void Cancel(long orderId, [FromBody]CancelRequest request)
        {
            var refundOrder = ChangeSchoolOrderService.GetRefundOrder(orderId);
            if (refundOrder == null)
            {
                throw new ArgumentNullException(nameof(refundOrder));
            }

            var service = new ChangeSchoolOrderService(base.SchoolId, refundOrder.StudentId);
            service.Cancel(orderId, base.CurrentUser.UserId, base.CurrentUser.UserName, request.Remark);
        }

        /// <summary>
        /// 本校转出-财务确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">财务确认数据</param>
        [HttpPost, Route("FinanceConfirm")]
        public void FinanceConfirm([FromBody]ChangeSchoolOrderConfirmRequest request)
        {
            var refundOrder = ChangeSchoolOrderService.GetRefundOrder(request.OrderId);
            if (refundOrder == null)
            {
                throw new ArgumentNullException(nameof(refundOrder));
            }

            request.OperatorId = base.CurrentUser.UserId;
            request.OperatorName = base.CurrentUser.UserName;

            var service = new ChangeSchoolOrderService(base.SchoolId, refundOrder.StudentId);
            service.FinanceConfirm(request);
        }

        /// <summary>
        /// 他校转入-接收/拒绝
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">转校接收数据源</param>
        [HttpPost, Route("Receive")]
        public void Receive([FromBody]ChangeSchoolOrderReceiveRequest request)
        {
            var refundOrder = ChangeSchoolOrderService.GetRefundOrder(request.OrderId);
            if (refundOrder == null)
            {
                throw new ArgumentNullException(nameof(refundOrder));
            }

            request.OperatorId = base.CurrentUser.UserId;
            request.OperatorName = base.CurrentUser.UserName;

            var service = new ChangeSchoolOrderService(base.SchoolId, refundOrder.StudentId);
            service.Receive(request);
        }
    }
}
