using AMS.API.Filter;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Service;
using AMS.Storage;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描述有:奖学金券业务逻辑API接口
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-2</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Coupon")]
    [ApiController]
    public class CouponController : BaseController
    {
        /// <summary>
        /// 根据查询条件获取奖学金券列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="seacher">奖学金查询条件</param>
        /// <returns>奖学金券分页列表</returns>
        [HttpGet, Route("GetCouponList")]
        [SchoolIdValidator]
        public PageResult<CouponInfoListResponse> GetCouponList([FromQuery] CouponRequest seacher)
        {
            return new CouponService(base.SchoolId).GetCouponList(seacher);
        }


        /// <summary>
        /// 描述：获取报名时要使用的我奖学金券列表（PC端使用）
        ///       包括转介绍和校长奖学金
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <returns>可以使用的优惠券列表</returns>
        [HttpGet, Route("GetEnrollCouponList")]
        [SchoolIdValidator]
        public List<EnrollCouponListResponse> GetEnrollCouponList(long studentId)
        {
            CouponService service = new CouponService(base.SchoolId);
            var result = service.GetEnrollCouponList(studentId);
            return result;
        }

        /// <summary>
        /// 描述：添加绑定校长奖学金券到学生（PC端使用）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-2</para>
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="couponNo"></param>
        [HttpGet, Route("BindCouponStudent")]
        [SchoolIdValidator]
        public void BindCouponStudent(long studentId, string couponNo)
        {
            CouponService service = new CouponService(base.SchoolId);
            service.BindCouponStudent(studentId, couponNo);
        }
    }
}
