using AMS.Dto;
using AMS.Storage.Models;
using System.Collections.Generic;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：订单交接接口
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public interface IOrderHandover
    {
        /// <summary>
        /// 获取未交接的统计列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <returns>订单未交接汇总列表</returns>
        List<OrderUnHandoverCountResponse> GetUnHandCountList();

        /// <summary>
        /// 获取未交接的列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>订单未交接列表</returns>
        List<OrderUnHandoverListResponse> GetUnHandList(string personalId);

        /// <summary>
        /// 获取未交接的订单列表（问题单与未交接）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>收款交接订单添加的实体列表</returns>
        List<OrderHandleAddResponse> GetUnHandleAddList(string personalId);

        /// <summary>
        /// 处理交接
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接数据对象</param>
        /// <returns>订单交接核对明细Id</returns>
        long Handle(OrderHandleAddRequest request);

        /// <summary>
        /// 将订单设为问题单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接数据对象</param>
        /// <returns>订单交接核对明细Id</returns>
        long MarkOrderProblem(OrderHandleAddRequest request);

        /// <summary>
        /// 获取下一需要交接的订单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="orderId">已核对的订单Id</param>
        /// <returns>收款交接订单添加的实体</returns>
        OrderHandleAddResponse GetNextHandoverOrder(string personalId, long orderId);

        /// <summary>
        /// 根据订单Id获取对应订单未交接详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="orderId">实体请求参数</param>
        /// <returns>订单未交接详情</returns>
        OrderUnHandoverDetailResponse GetUnHandoverDetail(long orderId);

        /// <summary>
        /// 获取已核对的未交接订单明细（按订单类型汇总）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="handoverDetails">已核对的未交接订单明细</param>
        /// <returns>订单类型的交接核对信息</returns>
        OrderHandoverTradeResponse GetHandoverTradeList(IEnumerable<TblFinOrderHandoverDetail> handoverDetails);

        /// <summary>
        /// 更新订单列表的状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="orderIdList">订单Id列表</param>
        /// <param name="status">订单状态</param>
        void UpdateOrderStatus(List<long> orderIdList, OrderStatus status);
    }
}
