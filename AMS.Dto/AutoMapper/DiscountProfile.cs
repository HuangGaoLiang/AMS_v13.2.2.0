using AMS.Dto.Enum;
using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscountProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public DiscountProfile()
        {

            CreateMap<TblDctCouponRule, CouponRuleListResponse>();

            CreateMap<TblDctCoupon, CouponInfoListResponse>().ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(CouponStatus), src.Status)))
                .ForMember(dest => dest.CouponTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(CouponType), src.CouponType)));

            CreateMap<TblDctCoupon, EnrollCouponListResponse>();    //报名获取优惠券

            CreateMap<TblDctCoupon, CouponResponse>();  //生成时返回优惠券的信息
        }
    }
}
