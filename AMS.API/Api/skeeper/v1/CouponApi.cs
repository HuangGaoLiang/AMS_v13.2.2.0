using AMS.API.Controllers;
using AMS.Dto;
using AMS.Dto.Enum;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Api.skeeper.v1
{
    /// <summary>
    /// 奖学金券业务逻辑API接口  ---App端使用
    /// </summary>
    [Produces("application/json"), Route("api/sKeeper/v1/Coupon")]
    [ApiController]
    public class CouponApi : BaseAppController
    {
        /// <summary>
        /// 生成奖学金券(添加) ---App使用
        /// ---瞿琦 20181030
        /// </summary>
        [HttpPost]
        public async Task<CouponResponse> Post([FromBody]CouponAmountRequest dto)
        {
            dto.CreatorId = base.CurrentUser.UserId;
            CouponService service = new CouponService(dto.SchoolId);
            return await service.CreateCoupon(dto);
        }

        /// <summary>
        /// 获取使用奖学金券列表(未使用/已使用/已过期) --App使用
        /// 瞿琦 20181030
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllCoupons")]
        public PageResult<CouponResponse> GetAllCoupons(int pageIndex, int pageSize)
        {
            return CouponService.GetAllCoupons(base.CurrentUser.UserId, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取奖学金券信息  ---App使用
        /// 瞿琦 20181030
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CouponResponse> Get(string schoolId,long couponId)
        {
            CouponService service = new CouponService(schoolId);
            return await service.GetCoupon(couponId);
        }

        /// <summary>
        /// 获取校长奖学金有效期
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("GetEffectiveDate")]
        public string GetEffectiveDate()
        {
            return  CouponService.GetEffectiveDate();
        }
    }
}
