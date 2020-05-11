using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.API.Filter;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Dto.AuditFlow;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述：赠与奖学金设置资源
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-10-29</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/CouponRule")]
    [ApiController]
    public class CouponRuleController :BaseController
    {
        /// <summary>
        /// 描述：获取赠与奖学金设置列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="request">列表筛选条件</param>
        /// <returns>审核通过的赠与奖学金列表</returns>
        [HttpGet, Route("GetList")]
        public PageResult<CouponRuleListResponse> GetList([FromQuery]CouponRuleListSearchRequest request)
        {
            request.SchoolNo = base.SchoolId;
            if (string.IsNullOrWhiteSpace(request.SchoolNo))
            {
                throw new BussinessException(ModelType.Discount, 3);
            }

            var result = CouponRuleService.GetList(request);
            return result;
        }

        /// <summary>
        /// 描述：获取状态
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <returns>当前赠与奖学金的审核状态</returns>
        [HttpGet, Route("CanSubmitToAudit")]
        [SchoolIdValidator]
        public int CanSubmitToAudit()
        {
            var result = new CouponRuleAuditService(base.SchoolId);
            return result.CurrentAuditStatus;
        }

        /// <summary>
        /// 描述：启用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="couponId">优惠券表Id</param>
        /// <returns>无</returns>
        [HttpGet, Route("SetEnable")]
        public void SetEnable(long couponId)
        {
            CouponRuleService.SetEnable(couponId);
        }
        /// <summary>
        /// 描述：禁用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="couponId">优惠券表Id</param>
        /// <returns>无</returns>
        [HttpGet, Route("SetDisabled")]
        public void SetDisabled(long couponId)
        {
            CouponRuleService.SetDisabled(couponId);
        }

        /// <summary>
        /// 描述：增加一条待提交审核的优惠记录
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="request">要添加的赠与奖学金信息</param>
        /// <returns>无</returns>
        [HttpPost]
        [SchoolIdValidator]
        public void Post(CouponRuleDetailRequest request)
        {
            var result = new CouponRuleAuditService(base.SchoolId);
            result.Add(request);
        }

        /// <summary>
        /// 描述：修改一条待提交审核的优惠记录
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="autCouponRuleId">赠与奖学金审核表Id</param>
        /// <param name="request">要修改的赠与奖学金表信息</param>
        /// <returns>无</returns>
        [HttpPut]
        [SchoolIdValidator]
        public void Put(long autCouponRuleId, CouponRuleDetailRequest request)
        {
            var result = new CouponRuleAuditService(base.SchoolId);
            result.Modify(autCouponRuleId, request);
        }

        /// <summary>
        /// 描述：删除一条待提交审核的优惠记录
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="autCouponRuleId">赠与奖学金审核表Id</param>
        /// <returns>无</returns>
        [HttpDelete]
        public void Delete(long autCouponRuleId)
        {
            CouponRuleAuditService.Remove(autCouponRuleId);
        }


        /// <summary>
        /// 描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="dto">要提交审核的赠与奖学金信息</param>
        /// <returns>无</returns>
        [HttpPost, Route("SubmitAudit")]
        [SchoolIdValidator]
        public void SubmitAudit(CouponRuleSubmitAuditRequest dto)
        {
            dto.CreateUserId = base.CurrentUser.UserId;
            dto.CreateUserName = base.CurrentUser.UserName;
            var result = new CouponRuleService(base.SchoolId);
            result.Audit(dto);
        }

        /// <summary>
        /// 描述：获取待提交审核、被退回及审核中的数据信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <returns>赠与奖学金审核信息</returns>
        [HttpGet, Route("GetAuditInfo")]
        [SchoolIdValidator]
        public CouponRuleAuditResponse GetAuditInfo()
        {
            var service = new CouponRuleAuditService(base.SchoolId);
            var result = service.GetAuditInfo();
            return result;
        }

        /// <summary>
        /// 描述：预览生成优惠数据（满减金额 报名时用）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <returns>优惠信息集合</returns>
        [HttpGet, Route("Peek")]
        [SchoolIdValidator]
        public async Task<List<CouponGenerateResponse>> Peek(long studentId,decimal totalAmount)
        {
            var service = new CouponRuleEnrollProducer(base.SchoolId, totalAmount);
            var result = await service.Peek(studentId);
            return result;
        }


        /// <summary>
        /// 描述：获取审核中的数据（平台审核端使用）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-27</para>
        /// </summary>
        /// <param name="auditId">审核Id</param>
        /// <returns>审核中的赠与奖学金信息列表</returns>
        [HttpGet,Route("GetAuditingCouponRuleList")]
        [SchoolIdValidator]
        public List<CouponRuleAuditDetailResponse> GetAuditingCouponRuleList(long auditId)
        {
            var auditService = CouponRuleAuditService.CreateByAutitId(auditId);
            var result = auditService.GetAuditingCouponRuleList();
            return result;
        }
    }
}
