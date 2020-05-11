using AMS.Models;
using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class OrdersProfile : Profile
    {
        /// <summary>
        /// 资金模块
        /// </summary>
        public OrdersProfile()
        {
            CreateMap<TblOdrDepositOrder, DepositOrderDetailResponse>()
                .ForMember(dest => dest.PayTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(PayType), src.PayType)))
                .ForMember(dest => dest.OrderStatuName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderStatus), src.OrderStatus)))
                .ForMember(dest => dest.UsesTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(UsesType), src.UsesType)));

            //报名排课
            CreateMap<MakeLessonOrderItemResponse, TblOdrEnrollOrderItem>();

            //报班查询
            CreateMap<TblOdrEnrollOrder, EnrollOrderListResponse>()
                .ForMember(d => d.PayTypeId, opt => opt.MapFrom(o => o.PayType))
                .ForMember(d => d.OrderStatusId, y => y.MapFrom(b => b.OrderStatus));

            //报班退费明细列表
            CreateMap<ViewLeaveClassDetail, LeaveClassOrderListResponse>();

            //报名详情
            CreateMap<EnrollOrderDetailResponse, EnrollPrintOrderDetailResponse>();

            //余额退费
            CreateMap<ViewBalanceRefundOrder, BalanceRefundOrderListResponse>().ForMember(dest => dest.RefundTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(RefundType), src.RefundType)));
            CreateMap<ViewBalanceRefundOrder, BalanceRefundOrderDetailResponse>().ForMember(dest => dest.RefundTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(RefundType), src.RefundType)));

            //报名详情
            CreateMap<StudentDetailResponse, StudentDetailResponse>();

            //报班
            CreateMap<ViewOdrEnrollOrder, EnrollOrderListResponse>()
                .ForMember(dest => dest.PayType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(PayType), src.PayTypeId)))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderStatus), src.OrderStatusId)))
                .ForMember(dest => dest.OrderNewType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderNewType), src.OrderNewType)));

            //订金列表
            CreateMap<ViewDepositOrder, DepositOrderListResponse>()
                .ForMember(dest => dest.PayTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(PayType), src.PayType)))
                .ForMember(dest => dest.UsesTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(UsesType), src.UsesType)))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderStatus), src.OrderStatus)))
                .ForMember(dest => dest.PayDate, opt => opt.MapFrom(src => src.CreateTime));

            //报名作废月
            CreateMap<ViewOrderObsoleteMonth, EnrollOrderMonthCancelListResponse>()
                .ForMember(dest => dest.PayType,
                    opt => opt.MapFrom(src => EnumName.GetDescription(typeof(PayType), src.PayType)))
                .ForMember(dest => dest.PayTypeId, opt => opt.MapFrom(src => src.PayType))
                .ForMember(dest => dest.OrderStatus,
                    opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderStatus), src.OrderStatus)))
                .ForMember(dest => dest.CreateName, opt => opt.MapFrom(src => src.Payee));

            //休学列表
            CreateMap<ViewLeaveSchoolOrder, LeaveSchoolOrderInfoResponse>();
        }
    }
}
