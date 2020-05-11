using AMS.Models;
using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class CstProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public CstProfile()
        {
            //1、查询学生信息
            CreateMap<TblCstStudent, StudentSearchResponse>();

            //2、学生信息详情
            CreateMap<TblCstStudent, StudentDetailResponse>();

            //3、学生列表
            CreateMap<TblCstStudent, StudentListResponse>();
            CreateMap<ViewStudent, StudentListResponse>()
                .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(SexEnum), src.Sex)))
                .ForMember(dest => dest.StudyStatuName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(StudyStatus), src.StudyStatus)));

            //3、学生在读状态
            CreateMap<OdrRefundOrderResponse, TblCstSchoolStudent>();
        }
    }
}
