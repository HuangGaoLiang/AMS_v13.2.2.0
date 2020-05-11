using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Dto.AuditFlow;
using AMS.Dto.Enum;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：表示一个校区的赠与奖学金设置
    /// <para>作    者：caiyakang</para>
    /// <para>创建时间：2018-10-24</para>
    /// </summary>
    public class CouponRuleService
    {
        private readonly string _schoolId;                                              //校区Id
        private readonly Lazy<CouponRuleAuditService> _auditService;                    //赠与奖学金审核服务
        private readonly Lazy<TblDctCouponRuleRepository> _tblDctCouponRuleRepository;  //赠与奖学金仓储

        /// <summary>
        /// 描述：实例化一个校区的赠与奖学金设置对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public CouponRuleService(string schoolId)
        {
            this._schoolId = schoolId;
            _auditService = new Lazy<CouponRuleAuditService>(() => { return new CouponRuleAuditService(this._schoolId); });
            _tblDctCouponRuleRepository = new Lazy<TblDctCouponRuleRepository>();
        }


        /// <summary>
        ///  描述：获取赠与奖学金设置列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="request">列表筛选条件</param>
        /// <returns>赠与奖学金分页列表</returns>

        public static PageResult<CouponRuleListResponse> GetList(CouponRuleListSearchRequest request)
        {
            var result = new PageResult<CouponRuleListResponse>();
            var query = new TblDctCouponRuleRepository().GetList(request);

            result.Data = Mapper.Map<List<TblDctCouponRule>, List<CouponRuleListResponse>>(query.Data);
            result.CurrentPage = query.CurrentPage;
            result.PageSize = query.PageSize;
            result.TotalData = query.TotalData;

            //判断奖学金是否生效
            var currentDate = DateTime.Now;
            foreach (var item in result.Data)
            {
                var effectType = CouponRuleStatus.Effect;

                if (item.BeginDate <= currentDate && currentDate <= item.EndDate.AddDays(1))
                {
                    effectType = CouponRuleStatus.Effect;
                }
                else if (item.BeginDate >= currentDate)
                {
                    effectType = CouponRuleStatus.WaitEffect;
                }
                else if (item.EndDate.AddDays(1) <= currentDate)
                {
                    effectType = CouponRuleStatus.NoEffect;
                }

                item.EffectType = effectType;
            }
            return result;
        }


        /// <summary>
        ///  描述：启用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：11, 异常描述:此时间段内已存在转介绍的优惠设置
        /// </exception>
        public static void SetEnable(long couponId)
        {
            //当前优惠券的信息
            var resultModel = new TblDctCouponRuleRepository().Load(x => x.CouponRuleId == couponId);
            //判断是否存在其他优惠券的信息
            var result = new CouponRuleService(resultModel.SchoolId).GetTypeByCouponRule(CouponType.Recommend)
                             .Where(x => x.CouponRuleId != couponId);
            if (resultModel.CouponType == (int)CouponType.Recommend)
            {
                if (result.Any(item => (item.BeginDate <= resultModel.BeginDate && resultModel.BeginDate <= item.EndDate) || (item.BeginDate <= resultModel.EndDate && resultModel.EndDate <= item.EndDate)))
                {
                    throw new BussinessException(ModelType.Discount, 11);
                }
            }
            SetSwitch(couponId, false);
        }


        /// <summary>
        ///  描述：禁用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <returns>无</returns>
        /// <param name="couponId">优惠券Id</param>
        public static void SetDisabled(long couponId)
        {
            SetSwitch(couponId, true);
        }


        /// <summary>
        ///  描述：开关
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="disabled">是否启用</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1, 异常描述:该奖学金信息不存在
        /// </exception>
        private static void SetSwitch(long couponId, bool disabled)
        {
            var resultModel = new TblDctCouponRuleRepository().Load(x => x.CouponRuleId == couponId);
            if (resultModel == null)
            {
                throw new BussinessException(ModelType.Discount, 1);
            }

            resultModel.IsDisabled = disabled;
            new TblDctCouponRuleRepository().Update(resultModel);
        }

        /// <summary>
        ///  描述：提交审核
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="dto">要提交审核的赠与奖学金信息</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：2, 异常描述:当前单据已在审核中
        /// 异常ID：14, 异常描述:当前登录人不是流程提交人
        /// </exception>
        public void Audit(CouponRuleSubmitAuditRequest dto)
        {
            //1.验证是否可以提交审核
            if (_auditService.Value.IsAuditing)
            {
                throw new BussinessException(ModelType.Discount, 2);
            }

            if (_auditService.Value.CurrentAuditStatus == (int)AuditStatus.Return && !_auditService.Value.IsFlowSubmitUser)
            {
                throw new BussinessException(ModelType.Discount, 14);
            }

            //2.向流程平台提交数据
            _auditService.Value.Audit(dto);
        }

        /// <summary>
        ///  描述：审核成功之后
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="entity">审核通过的赠与奖学金信息</param>
        /// <returns>无</returns>

        internal static void OnAuditSuccess(List<TblDctCouponRule> entity)
        {
            new TblDctCouponRuleRepository().Add(entity);
        }

        /// <summary>
        /// 描述：获取校区下某种类型的优惠政策
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="couponType">优惠券类型</param>
        /// <returns>赠与奖学金列表</returns>

        internal List<TblDctCouponRule> GetTypeByCouponRule(CouponType couponType)
        {
            var result = _tblDctCouponRuleRepository.Value.GetTypeByCouponRule(this._schoolId, couponType);
            return result;
        }

        /// <summary>
        /// 描述：获取本校区当前阶段下最新的转介绍优惠信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-1-2</para>
        /// </summary>
        /// <returns>赠与奖学金信息</returns>

        internal TblDctCouponRule GetTblDctCouponRuleInfo()
        {
            var entity = _tblDctCouponRuleRepository.Value.GetRecommendCouponRule(this._schoolId);
            return entity;
        }
        /// <summary>
        ///  描述：根据主键获取转介绍信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-1-2</para>
        /// </summary>
        /// <param name="couponRuleId">赠与奖学金表Id</param>
        /// <returns>赠与奖学金信息</returns>

        internal TblDctCouponRule GetCouponRuleIdByRuleList(long couponRuleId)
        {
            var result = _tblDctCouponRuleRepository.Value.GetCouponRuleIdByRuleList(couponRuleId).Result;
            return result;
        }
    }
}
