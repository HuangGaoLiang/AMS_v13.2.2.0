/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using AMS.Dto;
using AMS.Storage.Models;
using Jerrisoft.Platform.Public.PageExtensions;
using Jerrisoft.Platform.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Storage.Repository
{
    /// <summary>
    /// 描述：TblDctCoupon仓储
    /// <para>作  者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    public class TblDctCouponRepository : BaseRepository<TblDctCoupon>
    {
        /// <summary>
        /// 描述：实例化一个TblDctCoupon仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        public TblDctCouponRepository()
        {
        }
        /// <summary>
        /// 描述：实例化一个TblDctCoupon仓储对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public TblDctCouponRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 描述：获取所有的校长奖学金券
        /// <para>作  者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <returns>优惠券列表分页</returns>
        public PageResult<TblDctCoupon> GetAllCoupons(string creatorId, int pageIndex, int pageSize)
        {
            var result = base.LoadQueryable(x => x.CouponType == (int)CouponType.HeadmasterBonus, false)
                         .Where(x => x.CreatorId.Trim() == creatorId.Trim()).OrderByDescending(x => x.CreateTime)
                         .ToPagerSource(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// 描述：根据主键获取优惠券信息
        /// <para>作  者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <returns>优惠券列表</returns>
        public async Task<TblDctCoupon> GetCouponInfo(long couponId)
        {
            var result = await base.LoadTask(x => x.CouponId == couponId);
            return result;
        }

        /// <summary>
        /// 描述：根据主键获取优惠券信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="couponIdList">优惠券Id集合</param>
        /// <returns>优惠券列表</returns>
        public async Task<List<TblDctCoupon>> GetCouponInfo(IEnumerable<long> couponIdList)
        {
            var result = await base.LoadLisTask(x => couponIdList.Contains(x.CouponId));
            return result;
        }

        /// <summary>
        /// 描述：根据券号查找校长奖学金券信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="couponNo">优惠券号</param>
        /// <returns>优惠券信息</returns>
        public TblDctCoupon GetCouponNoByCouponInfo(string schoolId, string couponNo)
        {
            return base.Load(x => x.SchoolId.Trim() == schoolId.Trim() && x.CouponNo.Trim() == couponNo.Trim() && x.CouponType == (int)CouponType.HeadmasterBonus);
        }

        /// <summary>
        /// 描述：根据订单号获取优惠券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="enrollOrderIds">订单ID集合</param>
        /// <returns>优惠券集合</returns>
        public List<TblDctCoupon> GetEnrollOrderByCoupon(IEnumerable<long> enrollOrderIds)
        {
            return base.LoadList(x => enrollOrderIds.Contains(x.EnrollOrderId));
        }

        /// <summary>
        /// 描述：根据订单号获取优惠券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="enrollOrderId">订单ID</param>
        /// <returns>优惠券集合</returns>
        public List<TblDctCoupon> GetEnrollOrderByCoupon(long enrollOrderId)
        {
            return base.LoadList(x => enrollOrderId == x.EnrollOrderId);
        }

        /// <summary>
        /// 描述：获取学生优惠券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns></returns>
        public List<TblDctCoupon> GetCouponListByStudentId(string schoolId, long studentId)
        {
            var currentDate = DateTime.Now;
            var result = base.LoadList(x => x.StudentId == studentId && x.SchoolId.Trim() == schoolId.Trim()
                                     && x.Status == (int)Dto.Enum.CouponStatus.NoUse
                                     && x.ExpireTime >= currentDate);
            return result;
        }

        /// <summary>
        /// 描述：查找学生是否存在转介绍优惠券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>学生拥有优惠券的数量</returns>
        public int GetCouponByStudentIdCount(string schoolId, long studentId)
        {
            var result = base.LoadList(x => x.StudentId == studentId && x.SchoolId.Trim() == schoolId.Trim());
            return result.Count;
        }

        /// <summary>
        /// 分页获取奖学金列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">奖学金请求对象</param>
        /// <returns>奖学金分页列表</returns>
        public PageResult<TblDctCoupon> GetCouponList(string schoolId, CouponRequest request)
        {
            var result = base.LoadQueryable()
                        .Where(x => x.SchoolId == schoolId && x.StudentId == request.StudentId && x.Status != (int)AMS.Dto.Enum.CouponStatus.Invalid)
                        .OrderBy(x => x.Status).ThenByDescending(x => x.CreateTime);

            return result.ToPagerSource(request.PageIndex, request.PageSize);
        }

        /// <summary>
        /// 描述：修改优惠券的使用状态，使用时间及订单Id (校长奖学金才绑定订单Id，满减，转介绍时不修改)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="couponIdList">优惠券Id集合</param>
        /// <param name="status">优惠券状态</param>
        /// <param name="dateTime">使用时间</param>
        /// <param name="enrollOrderId">校长奖学金才绑定订单Id，满减，转介绍时不修改</param>
        /// <returns>是否修改成功的状态</returns>
        public async Task<bool> UpdateCouponAsync(IEnumerable<long> couponIdList, Dto.Enum.CouponStatus status, DateTime dateTime, long? enrollOrderId)
        {
            return await base.UpdateTask(x => couponIdList.Contains(x.CouponId), k => new TblDctCoupon
            {
                Status = (int)status,
                UseTime = dateTime,
                EnrollOrderId = enrollOrderId ?? k.EnrollOrderId
            });
        }

        /// <summary>
        /// 描述：修改优惠券的使用状态，使用时间及订单Id (校长奖学金才绑定订单Id，满减，转介绍时不修改)
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="couponId">优惠券Id</param>
        /// <param name="status">优惠券状态</param>
        /// <param name="dateTime">使用时间</param>
        /// <param name="enrollOrderId">校长奖学金才绑定订单Id，满减，转介绍时不修改</param>
        /// <returns>是否修改成功的状态</returns>
        public async Task<bool> UpdateCouponAsync(long couponId, Dto.Enum.CouponStatus status, DateTime dateTime, long? enrollOrderId)
        {
            return await base.UpdateTask(x => couponId == x.CouponId, k => new TblDctCoupon
            {
                Status = (int)status,
                UseTime = dateTime,
                EnrollOrderId = enrollOrderId ?? k.EnrollOrderId
            });
        }
    }
}
