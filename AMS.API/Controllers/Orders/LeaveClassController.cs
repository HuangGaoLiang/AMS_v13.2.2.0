using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：退班资源
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-9</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/LeaveClass")]
    [ApiController]
    public class LeaveClassController : BaseController
    {

        /// <summary>
        /// 描述：获取订单详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studenId">学生Id</param>
        /// <param name="orderId">退费订单表Id</param>
        /// <returns>退费订单详情</returns>
        [HttpGet, Route("GetOrderDetail")]
        [SchoolIdValidator]
        public LeaveClassOrderDetailResponse GetOrderDetail(long studenId,long orderId)
        {
            var service= new LeaveClassOrderService(base.SchoolId,studenId);
            var result= service.GetOrderDetail(orderId,base.CurrentUser.CompanyId) as LeaveClassOrderDetailResponse;
            return result;
        }


        /// <summary>
        /// 描述：获取要打印的收据详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studenId">学生Id</param>
        /// <param name="orderId">退费订单表Id</param>
        /// <returns>订单收据详情</returns>
        [HttpGet, Route("GetPrintReceiptDetail")]
        [SchoolIdValidator]
        public LeaveClassOrderReceiptDetailResponse GetPrintReceiptDetail(long studenId,long orderId)
        {
            var service = new LeaveClassOrderService(base.SchoolId,studenId);
            var result= service.GetPrintReceiptDetail(orderId,base.CurrentUser.CompanyId);
            return result;
        }



        /// <summary>
        /// 描述：办理前获取要办理的数据详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">退班时间</param>
        /// <returns>退班订单详情</returns>
        [HttpGet, Route("GetLeaveClassOrderTransactDetail")]
        [SchoolIdValidator]
        public LeaveClassOrderTransactDetailResponse GetLeaveClassOrderTransactDetail(long studentId, [FromQuery]LeaveClassOrderTransactDetailRequest request)
        {
            var service = new LeaveClassOrderService(base.SchoolId, studentId);
            var result = service.GetTransactDetail(request, base.CurrentUser.CompanyId) as LeaveClassOrderTransactDetailResponse;
            return result;
        }

        /// <summary>
        /// 描述：办理退班
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">要添加的退班信息</param>
        [HttpPost]
        [SchoolIdValidator]
        public void Post(long studentId, [FromBody]LeaveClassOrderAddRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            var service = new LeaveClassOrderService(base.SchoolId, studentId);
            service.Transact(request,base.CurrentUser.CompanyId);
        }


        /// <summary>
        /// 描述：获取退班列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">列表筛选条件</param>
        /// <returns></returns>
        [HttpGet, Route("GetLeaveClassOrderList")]
        [SchoolIdValidator]
        public PageResult<LeaveClassOrderListResponse> GetLeaveClassOrderList(long studentId, [FromQuery]LeaveClassOrderListSearchRequest request)
        {
            request.SchoolId = base.SchoolId;
            var service = new LeaveClassOrderService(base.SchoolId, studentId);
            var result = service.GetLeaveClassOrderList(request);
            return result;
        }
    }
}
