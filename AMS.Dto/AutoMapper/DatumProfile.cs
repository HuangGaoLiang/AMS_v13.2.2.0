using System.Collections.Generic;
using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 基础配置模块映射
    /// </summary>
    public class DatumProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public DatumProfile()
        {
            //1、添加上课时间段
            CreateMap<SchoolTimeSaveRequest, TblDatSchoolTime>();

            //2、学期收费标准提交审核
            CreateMap<TermAuditDetailRequest, TblDatTerm>();
            CreateMap<TermAuditDetailRequest, TblAutTerm>();

            //3、课程级别
            CreateMap<CourseLevelResponse, TblDatCourseLevel>();

            //4、添加课程级别
            CreateMap<CourseLevelAddRequest, TblDatCourseLevel>();

            //5、教室分配及学位设置列表 entity to dto
            CreateMap<ViewRoomCourse, RoomCourseResponse>();

            //6、课程添加的数据  dto to entity
            CreateMap<CourseAddRequest, TblDatCourse>();

            //7、课程entity=>课程列表dto
            CreateMap<TblDatCourse, CourseListResponse>();
            CreateMap<TblDatCourse, CourseResponse>()
                 .ForMember(dest => dest.CourseLevels, opt => opt.MapFrom(src => new List<CourseLevelMiddleResponse>()));
            CreateMap<TblDatCourse, CourseShortResponse>();

            //8、附件相关  dto to entity
            CreateMap<TblDatAttchment, AttchmentDetailResponse>();

            //9、投诉与建议列表
            CreateMap<TblDatFeedback, FeedbackListResponse>()
              .ForMember(dest => dest.ProcessStatusName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(FeedbackProcessStatus), src.ProcessStatus)));

            //9、投诉与建议详情
            CreateMap<TblDatFeedback, FeedbackDetailResponse>()
                .ForMember(dest => dest.ProcessStatusName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(FeedbackProcessStatus), src.ProcessStatus)));
        }
    }
}
