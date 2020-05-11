using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class CashProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public CashProfile()
        {
            CreateMap<TblCashWalletTrade, WalletTradeListResponse>().ForMember(dest => dest.TradeType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderTradeType), src.TradeType)));
            CreateMap<TblCashOrderTrade, CashOrderTradeListResponse>().ForMember(dest => dest.TradeTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(OrderTradeType), src.TradeType)));
        }
    }
}
