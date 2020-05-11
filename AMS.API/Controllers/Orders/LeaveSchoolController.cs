using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：休学资源
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-6</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/LeaveSchool")]
    [ApiController]
    [SchoolIdValidator]
    public class LeaveSchoolController : BaseController
    {

        /// <summary>
        /// 描述：获取休学明细列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="iRequest">列表筛选条件</param>
        /// <returns>休学明细列表</returns>
        [HttpGet, Route("GetOrderList")]
        public LeaveSchoolOrderListResponse GetOrderList([FromQuery]LeaveSchoolOrderListSearchRequest iRequest)
        {
            
            var service = new LeaveSchoolOrderService(base.SchoolId);
            var leaveSchoolQuery = service.GetOrderList(iRequest);

            return leaveSchoolQuery;
        }


        /// <summary>
        /// 描述：根据休学日期获取当前学生费用明细
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-6</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">休学时间</param>
        /// <returns>要休学的订单课程明细</returns>
        [HttpGet, Route("GetLeaveSchoolOrderTransactDetail")]
        public LeaveSchoolOrderTransactDetailResponse GetLeaveSchoolOrderTransactDetail(long studentId, [FromQuery]LeaveClassOrderTransactDetailRequest request)
        {
            var service = new LeaveSchoolOrderService(base.SchoolId, studentId);
            var result = service.GetTransactDetail(request, base.CurrentUser.CompanyId) as LeaveSchoolOrderTransactDetailResponse;
            return result;
        }



        /// <summary>
        /// 描述：休学办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-6</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">要保存的休学信息</param>
        /// <returns>无</returns>
        [HttpPost]
        public void Post(long studentId, [FromBody]LeaveSchoolOrderAddRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            var service = new LeaveSchoolOrderService(base.SchoolId, studentId);
            service.Transact(request, base.CurrentUser.CompanyId);
        }




        /// <summary>
        /// 描述：获取订单详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="refundOrderId">退费订单表Id</param>
        /// <returns>休学订单详情</returns>
        [HttpGet, Route("GetOrderDetail")]
        public LeaveSchoolOrderDetailResponse GetOrderDetail(long studentId, long refundOrderId)
        {
            var service = new LeaveSchoolOrderService(base.SchoolId, studentId);
            var result = service.GetOrderDetail(refundOrderId, base.CurrentUser.CompanyId) as LeaveSchoolOrderDetailResponse;
            return result;
        }
    }
}
