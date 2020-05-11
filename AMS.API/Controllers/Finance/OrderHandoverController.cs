using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：订单交接业务逻辑API接口
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-15</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/OrderHandover")]
    [ApiController]
    public class OrderHandoverController : BsnoController
    {
        /// <summary>
        /// 获取校区未交接订单汇总列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <returns>订单未交接汇总列表</returns>
        [HttpGet, Route("GetUnHandCountList")]
        public List<OrderUnHandoverCountResponse> GetUnHandCountList()
        {
            return new OrderHandoverService(base.SchoolId).GetUnHandCountList();
        }

        /// <summary>
        /// 根据招生专员Id获取校区未交接订单列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>订单未交接分页列表</returns>
        [HttpGet, Route("GetUnHandList")]
        public PageResult<OrderUnHandoverListResponse> GetUnHandList(string personalId, int pageIndex, int pageSize)
        {
            return new OrderHandoverService(base.SchoolId).GetUnHandList(personalId, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取校区接下来要处理的未交接订单列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderTradeType">订单类型</param>
        /// <returns>收款交接要添加的订单信息</returns>
        [HttpGet, Route("GetOrderHandleAddList")]
        public List<OrderHandleAddResponse> GetOrderHandleAddList(string personalId, long orderId, int orderTradeType)
        {
            return new OrderHandoverService(base.SchoolId).GetOrderHandleAddList(personalId, orderId, orderTradeType);
        }

        /// <summary>
        /// 核对未交接订单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接请求对象</param>
        /// <returns>收款交接要添加的订单信息</returns>
        [HttpPost]
        public OrderHandleAddResponse Post(OrderHandleAddRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            request.SchoolId = base.SchoolId;
            return new OrderHandoverService(base.SchoolId).Handle(request);
        }

        /// <summary>
        /// 将订单设为问题单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接请求对象</param>
        /// <returns>收款交接要添加的订单信息</returns>
        [HttpPost, Route("MarkOrderProblem")]
        public OrderHandleAddResponse MarkOrderProblem(OrderHandleAddRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            request.SchoolId = base.SchoolId;
            return new OrderHandoverService(base.SchoolId).MarkOrderProblem(request);
        }

        /// <summary>
        /// 获取校区已交接订单列表
        /// 只允许查询最多一个月时间段
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">订单已交接查询条件</param>
        /// <returns>订单已交接分页列表</returns>
        [HttpGet, Route("GetHandList")]
        public PageResult<OrderHandoverListResponse> GetHandList([FromQuery]OrderHandoverListSearchRequest request)
        {
            request.SchoolId = base.SchoolId;
            return new OrderHandoverService(base.SchoolId).GetHandList(request);
        }

        /// <summary>
        /// 获取校区已交接订单列表
        /// 只允许查询最多一个月时间段
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">订单交接明细配查询条件</param>
        /// <returns>订单交接明细信息分页列表</returns>
        [HttpGet, Route("GetHandDetailList")]
        public PageResult<OrderHandoverDetailListResponse> GetHandDetailList([FromQuery]OrderHandoverDetailRequest request)
        {
            request.SchoolId = base.SchoolId;
            //校区Id是招生专员对应的校区Id
            return new OrderHandoverService(base.SchoolId).GetHandDetailList(request);
        }

        /// <summary>
        /// 查看收款未交接详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="tradeType">交易类型</param>
        /// <returns>订单未交接详情</returns>
        [HttpGet, Route("Preview")]
        public OrderUnHandoverDetailResponse Preview(long orderId, int tradeType)
        {
            return new OrderHandoverService(base.SchoolId).GetUnHandoverDetail(orderId, tradeType);
        }

        /// <summary>
        /// 预览收款交接表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>收款交接表信息</returns>
        [HttpGet, Route("PreviewReport")]
        public OrderHandoverReportResponse PreviewReport(string personalId)
        {
            return new OrderHandoverService(base.SchoolId).GetHandoverListByPersonalId(personalId);
        }

        /// <summary>
        /// 生成收款交接表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="request">收款交接信息</param>
        [HttpPost, Route("SaveReport")]
        public async Task SaveReport(OrderHandoverRequest request)
        {
            request.CreatorId = base.CurrentUser.UserId;
            request.CreatorName = base.CurrentUser.UserName;
            request.CreateTime = request.HandoverDate = DateTime.Now;
            request.SchoolId = base.SchoolId;
            await new OrderHandoverService(base.SchoolId).SaveReport(request);
        }

        /// <summary>
        /// 查看收款交接表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="handoverId">收款交接Id</param>
        /// <returns>收款交接表信息</returns>
        [HttpGet, Route("GetReport")]
        public OrderHandoverReportResponse GetReport(long handoverId)
        {
            return new OrderHandoverService(base.SchoolId).GetHandoverListByHandoverId(handoverId);
        }

        /// <summary>
        /// 检查招生专员是否存在已核对的收款交接信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        [HttpGet, Route("IsExistOrderHandover")]
        public void IsExistOrderHandover(string personalId)
        {
            new OrderHandoverService(base.SchoolId).IsExistOrderHandover(personalId);
        }
    }
}
