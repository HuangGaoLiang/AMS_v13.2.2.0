using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 报名资源
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/EnrollOrder")]
    [ApiController]
    public class EnrollOrderController : BsnoController
    {
        /// <summary>
        /// 提交学生报名
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08 </para>
        /// </summary>
        /// <param name="request">提交报名数据</param>
        [HttpPost]
        public string Post([FromBody]EnrollOrderRequest request)
        {
            return new EnrollOrderService(base.SchoolId).Enrolling(request, base.CurrentUser.UserId, base.CurrentUser.UserName, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 获取报班订单分页列表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08 </para>
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns>返回表名订单分页数据</returns>
        [HttpGet, Route("GetPageList")]
        public PageResult<EnrollOrderListResponse> GetPageList([FromQuery]EnrollOrderListSearchRequest dto)
        {
            dto.SchoolId = base.SchoolId;
           
            return new EnrollOrderService(base.SchoolId).GetPageList(dto);
        }

        /// <summary>
        /// 根据订单编号获取报名订单详情(报班收款详情)
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08</para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>返回表名订单详细数据</returns>
        [HttpGet]
        public EnrollOrderDetailResponse Get(long orderId)
        {
            return (EnrollOrderDetailResponse)new EnrollOrderService(base.SchoolId).GetOrderDetail(orderId,base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 打印收据
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08</para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>返回表名订单详细数据</returns>
        [HttpGet, Route("GetPrintOrderDetail")]
        public EnrollOrderDetailResponse GetPrintOrderDetail(long orderId)
        {
            return new EnrollOrderService(base.SchoolId).GetPrintOrderDetail(orderId, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 打印报名表
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08 </para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>返回表名订单详细数据</returns>
        [HttpGet, Route("GetPrintEnrollOrder")]
        public EnrollPrintOrderDetailResponse GetPrintEnrollOrder(long orderId)
        {
            return new EnrollOrderService(base.SchoolId).GetPrintEnrollOrder(orderId, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 作废报名订单－－当天作废
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08</para>
        /// </summary>
        /// <param name="enrollOrderId">订单编号</param>
        /// <param name="cancelRemark">作废备注</param>
        [HttpPut, Route("CancelToday")]
        public void CancelToday(long enrollOrderId, string cancelRemark)
        {
            new EnrollOrderService(base.SchoolId).CancelToday(enrollOrderId, base.CurrentUser.UserId, base.CurrentUser.UserName, cancelRemark);
        }

        /// <summary>
        /// 报名作废（月）列表（获取未交接的报名列表）
        ///  <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08</para>
        /// </summary>
        /// <param name="stuName">学生姓名</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>返回报名月分页数据</returns>
        [HttpGet, Route("GetEnrollOrderMonthCancelList")]
        public PageResult<EnrollOrderMonthCancelListResponse> GetEnrollOrderMonthCancelList(string stuName, int pageIndex, int pageSize)
        {
            return new EnrollOrderService(base.SchoolId).GetEnrollOrderMonthCancelList(stuName, base.SchoolId, pageIndex, pageSize);
        }

        /// <summary>
        /// 作废报名订单－－当月作废
        ///  <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-11-08</para>
        /// </summary>
        /// <param name="enrollOrderId">订单编号</param>
        /// <param name="cancelRemark">作废备注呢</param>
        [HttpPut, Route("CancelCurrentMonth")]
        public void CancelCurrentMonth(long enrollOrderId, string cancelRemark)
        {
            new EnrollOrderService(base.SchoolId).CancelCurrentMonth(enrollOrderId, base.CurrentUser.UserId, base.CurrentUser.UserName, cancelRemark);
        }

    }
}
