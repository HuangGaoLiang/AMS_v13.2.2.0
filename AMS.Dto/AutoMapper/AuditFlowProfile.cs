using AMS.Models;
using AMS.Storage.Models;
using AutoMapper;
using System.Collections.Generic;
using AMS.Dto.Dto.AuditFlow;
using YMM.Storage.Models;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 审核模块映射
    /// </summary>
    public class AuditFlowProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AuditFlowProfile()
        {
            //校区排课 保存并提交审核 班级课表
            CreateMap<TblDatClass, ComparedDataResponse>().ForMember(dest => dest.AutClassId, opt => opt.MapFrom(src => new List<ComparedDataResponse>())); ;
            CreateMap<TblAutClass, ComparedDataResponse>();
            CreateMap<ComparedDataResponse, TblAutClass>();

            //班级上课时间表
            CreateMap<TblTimClassTime, AutClassTimeResponse>();
            CreateMap<TblAutClassTime, TblTimClassTime>();
            CreateMap<AutClassTimeResponse, TblAutClassTime>();

            //课表复制
            CreateMap<TblDatClass, TblAutClass>();
            CreateMap<TblTimClassTime, TblAutClassTime>();

            CreateMap<CouponRuleDetailRequest, TblAutCouponRule>();   //提交审核的赠与奖学金
            CreateMap<TblAutCouponRule, CouponRuleAuditDetailResponse> (); //获取审核中的赠与奖学金

        }
    }
}
