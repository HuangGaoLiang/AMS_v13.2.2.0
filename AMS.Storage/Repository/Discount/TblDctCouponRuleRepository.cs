/*此代码由生成工具字段生成，生成时间2018/10/27 16:53:11 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblDctCouponRule仓储
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-10-29</para>
    /// </summary>
    public class TblDctCouponRuleRepository : BaseRepository<TblDctCouponRule>
    {
        /// <summary>
        /// 描述：实例化一个TblDctCouponRule仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        public TblDctCouponRuleRepository()
        {
        }
        /// <summary>
        /// 描述：实例化一个TblDctCouponRule仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDctCouponRuleRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：获取审核通过的赠与奖学金列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="request">筛选条件集合</param>
        /// <returns>赠与奖学金列表分页</returns>
        public PageResult<TblDctCouponRule> GetList(CouponRuleListSearchRequest request)
        {
            var currentDate = DateTime.Now;
            var result = base.LoadQueryable()
                        .WhereIf(!string.IsNullOrWhiteSpace(request.SchoolNo), x => x.SchoolId.Trim() == request.SchoolNo.Trim())
                        .WhereIf(request.CouponType != 0, x => x.CouponType == (byte)request.CouponType) //优惠类型
                        .WhereIf(!string.IsNullOrWhiteSpace(request.CouponRuleName), x => x.CouponRuleName.Contains(request.CouponRuleName)) //优惠名称
                        .WhereIf(request.EffectType == CouponRuleStatus.Effect, x => x.BeginDate <= currentDate && currentDate <= x.EndDate)  //奖学金生效
                        .WhereIf(request.EffectType == CouponRuleStatus.NoEffect, x => x.EndDate < currentDate)                               //奖学金失效
                        .WhereIf(request.EffectType == CouponRuleStatus.WaitEffect, x => x.BeginDate > currentDate)                           //奖学金待生效
                        .OrderBy(x => x.IsDisabled).ThenByDescending(x => x.CreateTime);                                    //先启用排前面，然后创建时间倒序

            return result.ToPagerSource(request.PageIndex, request.PageSize);
        }
        /// <summary>
        /// 描述：获取赠与奖学金优惠金额(满减)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="totalAmount">交易金额</param>
        /// <returns>赠与奖学金列表</returns>
        public async Task<List<TblDctCouponRule>> GetAmountByCouponRule(string schoolId, decimal totalAmount)
        {
            var currentDate = DateTime.Now;
            var result = (await base.LoadLisTask(x => x.SchoolId.Trim() == schoolId.Trim()
                                                   && x.CouponType == (byte)CouponType.FullReduce
                                                   && x.FullAmount <= totalAmount
                                                   && x.IsDisabled == false
                                                   && ((x.MaxQuota > 0 && x.UseQuota < x.MaxQuota) || x.MaxQuota == 0)
                                                   && x.BeginDate <= currentDate && currentDate <= x.EndDate.AddDays(1)
                                                 )).OrderByDescending(x => x.CouponAmount).ToList();
            return result;
        }

        /// <summary>
        /// 描述：根据主键获取赠与奖学金信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="couponRuleId">赠与奖学金表Id</param>
        /// <returns>赠与奖学金信息</returns>
        public async Task<TblDctCouponRule> GetCouponRuleIdByRuleList(long couponRuleId)
        {
            var result = await base.LoadTask(x => x.CouponRuleId == couponRuleId);
            return result;
        }


        /// <summary>
        /// 描述：根据主键获取赠与奖学金信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="couponRuleId">赠与奖学金表Id</param>
        /// <returns>赠与奖学金列表</returns>
        public async Task<List<TblDctCouponRule>> GetCouponRuleIdByRuleList(IEnumerable<long> couponRuleId)
        {
            var result = await base.LoadLisTask(x => couponRuleId.Contains(x.CouponRuleId));
            return result;
        }


        /// <summary>
        /// 描述：获取有效的优惠金额信息（转介绍）
        ///      （因为同一时间段只有一个转介绍优惠，所有直接返回实体，没有转介绍优惠则返回null）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <returns>赠与奖学金信息</returns>
        public TblDctCouponRule GetRecommendCouponRule(string schoolId)
        {
            var currentDate = DateTime.Now;
            var result = base.Load(x => x.SchoolId.Trim() == schoolId.Trim()
                                && x.CouponType == (byte)CouponType.Recommend
                                && x.IsDisabled == false
                                && ((x.MaxQuota > 0 && x.UseQuota < x.MaxQuota) || x.MaxQuota == 0)
                                && x.BeginDate <= currentDate && currentDate <= x.EndDate.AddDays(1));
            return result;
        }

        /// <summary>
        /// 描述：根据主键修改使用人数（+1或-1）
        ///       (不能用批量，退回优惠券的时候主键会重复)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="couponRuleIdList">赠与奖学金Id集合</param>
        /// <param name="number">使用人数</param>
        /// <returns>是否执行成功</returns>
        public async Task<bool> UpdateUseQuotaAsync(IEnumerable<long> couponRuleIdList, int number)
        {
            var flag = true;
            foreach (var item in couponRuleIdList)
            {
                flag = await base.UpdateTask(x => item == x.CouponRuleId, k => new TblDctCouponRule { UseQuota = k.UseQuota + number > 0 ? k.UseQuota + number : 0 });
            }
            return flag;
        }

        /// <summary>
        /// 描述：根据校区和优惠类型获取优惠信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="couponType">优惠券类型</param>
        /// <returns>赠与奖学金列表</returns>
        public List<TblDctCouponRule> GetTypeByCouponRule(string schoolId, CouponType couponType)
        {
            var result = base.LoadList(x => x.SchoolId.Trim() == schoolId.Trim() && x.CouponType == (int)couponType && x.IsDisabled == false);
            return result;
        }

    }
}
