using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描述：表示一个校区的优惠券服务 
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-4</para>
    /// </summary>
    public class CouponService : BService
    {
        private readonly string _schoolId;                                      //校区Id     
        private readonly Lazy<TblDctCouponRepository> _tblDctCouponRepository;  //优惠券表仓储

        /// <summary>
        /// 描述：实例化一个校区的优惠券服务仓储
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public CouponService(string schoolId)
        {
            this._schoolId = schoolId;
            _tblDctCouponRepository = new Lazy<TblDctCouponRepository>();
        }

        /// <summary>
        /// 获取学生奖学金余额
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>学生奖学金总金额</returns>
        public decimal GetCouponBalanceAmount(long studentId)
        {
            var couponList = _tblDctCouponRepository.Value.GetCouponListByStudentId(this._schoolId, studentId);
            if (couponList != null && couponList.Count > 0)
            {
                return couponList.Sum(a => a.Amount);
            }
            return 0;
        }

        /// <summary>
        /// 获取奖学金券列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="request">奖学金查询条件</param>
        /// <returns>获取奖学金券分页列表</returns>
        public PageResult<CouponInfoListResponse> GetCouponList(CouponRequest request)
        {
            var result = new PageResult<CouponInfoListResponse>() { Data = new List<CouponInfoListResponse>() };
            var couponList = _tblDctCouponRepository.Value.GetCouponList(this._schoolId, request);
            if (couponList != null && couponList.Data != null && couponList.Data.Count > 0)
            {
                result.Data = Mapper.Map<List<CouponInfoListResponse>>(couponList.Data);
                result.CurrentPage = couponList.CurrentPage;
                result.PageSize = couponList.PageSize;
                result.TotalData = couponList.TotalData;
            }
            return result;
        }


        /// <summary>
        /// 描述：获取报名时可以使用的我奖学金券列表
        ///       包括转介绍和校长奖学金
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>可使用的奖学金列表</returns>
           
        public List<EnrollCouponListResponse> GetEnrollCouponList(long studentId)
        {
            var couponList = _tblDctCouponRepository.Value.GetCouponListByStudentId(this._schoolId, studentId);
            var result = Mapper.Map<List<EnrollCouponListResponse>>(couponList);
            return result;
        }

        /// <summary>
        /// 描述：添加绑定校长奖学金券到学生
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="couponNo">优惠券号</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：6, 异常描述:券号有误，请重新输入
        /// 异常ID：10, 异常描述:券号已被使用
        /// </exception>
        public void BindCouponStudent(long studentId, string couponNo)
        {
            var couponInfo = _tblDctCouponRepository.Value.GetCouponNoByCouponInfo(this._schoolId, couponNo);
            if (couponInfo == null)
            {
                throw new BussinessException(ModelType.Discount, 6);
            }
            if (couponInfo.StudentId > 0 && couponInfo.UseTime.HasValue)
            {
                throw new BussinessException(ModelType.Discount, 10);
            }

            couponInfo.StudentId = studentId;
            couponInfo.UseTime = DateTime.Now;

            _tblDctCouponRepository.Value.Update(couponInfo);
        }

        /// <summary>
        /// 描述：生成奖学金券
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <returns>生成的校长奖学金信息</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：4, 异常描述:添加校长奖学金失败
        /// 异常ID：5, 异常描述:优惠券号重复
        /// </exception>
        public async Task<CouponResponse> CreateCoupon(CouponAmountRequest dto)
        {
            var couponNo = CreateCouponNo.GetCouponCode();
            if (_tblDctCouponRepository.Value.GetCouponNoByCouponInfo(this._schoolId, couponNo) != null)
            {
                throw new BussinessException(ModelType.Discount, 5);
            }

            var entity = new TblDctCoupon()
            {
                CouponId = IdGenerator.NextId(),
                SchoolId = _schoolId,
                CouponNo = couponNo,
                CouponType = (int)CouponType.HeadmasterBonus,
                Amount = dto.Amount,
                Status = (int)Dto.Enum.CouponStatus.NoUse,
                ExpireTime = DateTime.Now.AddMonths(6),
                EnrollOrderId = 0,
                IsFreeAll = dto.IsFreeAll,
                StudentId = 0,
                UseTime = null,
                CreateTime = DateTime.Now,
                FromId = 0,
                Remark = string.Empty,
                CreatorId = dto.CreatorId
            };
            var flag = await _tblDctCouponRepository.Value.AddTask(entity);

            if (!flag)
            {
                throw new BussinessException(ModelType.Discount, 4);
            }
            var result = Mapper.Map<CouponResponse>(entity);
            OrgService orgService = new OrgService();
            var schoolList = orgService.GetAllSchoolList().FirstOrDefault(x => x.SchoolId.Trim() == _schoolId.Trim());
            result.SchoolName = schoolList == null ? "" : schoolList.SchoolName;
            return result;

        }

        /// <summary>
        /// 描述：获取所有奖学金券（未使用/已使用/已过期）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <returns>校长奖学金列表分页</returns>
           
        public static PageResult<CouponResponse> GetAllCoupons(string currentUserId, int pageIndex, int pageSize)
        {

            var currentDate = DateTime.Now;
            var query = new TblDctCouponRepository().GetAllCoupons(currentUserId, pageIndex, pageSize);

            var result = new PageResult<CouponResponse>();

            result.Data = Mapper.Map<List<CouponResponse>>(query.Data);
            result.PageSize = query.PageSize;
            result.TotalData = query.TotalData;
            result.CurrentPage = query.CurrentPage;

            var studentInfoList = StudentService.GetStudentByIds(result.Data.Select(x => x.StudentId)).Result;

            var schoolList = new OrgService().GetAllSchoolList();
            foreach (var item in result.Data)
            {

                if (item.Status == Dto.CouponStatus.HasUse) //已使用
                {
                    item.StatusName = EnumName.GetDescription(typeof(Dto.CouponStatus), Dto.CouponStatus.HasUse);
                }
                else if (item.Status == Dto.CouponStatus.NoUse && currentDate <= item.ExpireTime)  //未使用
                {
                    item.StatusName = EnumName.GetDescription(typeof(Dto.CouponStatus), Dto.CouponStatus.NoUse);
                }
                else if (item.Status == Dto.CouponStatus.NoUse && item.ExpireTime < currentDate)   //已过期
                {
                    item.StatusName = EnumName.GetDescription(typeof(Dto.CouponStatus), Dto.CouponStatus.NoEffect);
                    item.Status = Dto.CouponStatus.NoEffect;
                }

                item.StudentName = studentInfoList.FirstOrDefault(x => x.StudentId == item.StudentId)?.StudentName;
                item.SchoolName = schoolList.FirstOrDefault(x => x.SchoolId.Trim() == item.SchoolId.Trim())?.SchoolName;
            }

            return result;
        }

        /// <summary>
        /// 描述：获取单个奖学金券信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <returns>校长奖学金信息</returns>
           
        public async Task<CouponResponse> GetCoupon(long couponId)
        {
            var result = await _tblDctCouponRepository.Value.GetCouponInfo(couponId);
            var query = Mapper.Map<CouponResponse>(result);

            var schoolList = new OrgService().GetAllSchoolList();
            query.SchoolName = schoolList.FirstOrDefault(x => x.SchoolId.Trim() == query.SchoolId.Trim())?.SchoolName;
            return query;
        }


        /// <summary>
        /// 描述：根据一组优惠券ID获取优惠券信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <param name="couponIds">一组优惠券ID</param>
        /// <returns>优惠券列表</returns>
           
        internal async Task<List<TblDctCoupon>> GetById(List<long> couponIds)
        {
            return await _tblDctCouponRepository.Value.GetCouponInfo(couponIds);
        }

        /// <summary>
        ///  描述：获取校长奖学金有效期
        ///  <para>作    者：瞿琦</para>
        ///  <para>创建时间：2018-11-4</para>
        /// </summary>
        /// <returns>当前时间字符串</returns>
           
        public static string GetEffectiveDate()
        {
            var currentDate = DateTime.Now.AddMonths(6);
            return currentDate.ToString("yyyy-MM-dd");
        }
    }
}
