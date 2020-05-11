using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 定金资源
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/DepositOrder")]
    [ApiController]
    public class DepositOrderController : BsnoController
    {
        /// <summary>
        /// 根据条件查询定金表列
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-30</para>
        /// </summary>
        /// <param name="seacher">定金的查询条件</param>
        /// <returns>返回订金分页数据</returns>
        [HttpGet, Route("GetList")]
        public PageResult<DepositOrderListResponse> GetList([FromQuery] DepositOrderListSearchRequest seacher)
        {
            seacher.SchoolId = base.SchoolId;
            return new DepositOrderService(base.SchoolId).GetList(seacher);
        }

        /// <summary>
        /// 根据订金编号获取定金详情信息
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-30</para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <returns>返回订金详细数据</returns>
        [HttpGet]
        public DepositOrderDetailResponse Get(long depositOrderId)
        {
            return new DepositOrderService(base.SchoolId).GetDetail(depositOrderId,base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 打印订金
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-30</para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <returns>返回订金详细数据</returns>
        [HttpGet, Route("GetPrintOrderDetail")]
        public DepositOrderDetailResponse GetPrintOrderDetail(long depositOrderId)
        {
            return new DepositOrderService(base.SchoolId).GetPrintOrderDetail(depositOrderId, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 添加订金
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-30</para>
        /// </summary>
        /// <param name="request">输入参数</param>
        [HttpPost]
        public string Post(DepositOrderAddRequest request)
        {
            return new DepositOrderService(base.SchoolId).Add(request, base.CurrentUser.UserId, base.CurrentUser.UserName);
        }

        /// <summary>
        /// 作废定金
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-10-30</para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <param name="cancelRemark">作废原因</param>
        [HttpPost, Route("Cancel")]
        public async Task Cancel(long depositOrderId, string cancelRemark)
        {
            await new DepositOrderService(base.SchoolId).Cancel(depositOrderId, base.CurrentUser.UserId, base.CurrentUser.UserName, cancelRemark);
        }

    }
}
