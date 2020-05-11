using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AMS.Storage.Models;
using AMS.Models;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 课表模块映射
    /// </summary>
    public class TimetableProfile : Profile
    {
        public TimetableProfile()
        {
            //1、复制课表上课时间段
            CreateMap<DatSchoolTimResponse, TblDatSchoolTime>();

            CreateMap<ViewTimLifeClass, LifeClassLessonListResponse>().ForMember(dest => dest.BusinessType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(LessonBusinessType), src.BusinessType)))
                .ForMember(dest => dest.IDType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(DocumentType), src.IDType)))
                .ForMember(dest => dest.LessonType, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(LessonType), src.LessonType)))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(SexEnum), src.Sex)));

            CreateMap<ViewTimLifeClass, LifeClassLessonStudentExportResponse>()
                .ForMember(dest => dest.IdTypeName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(DocumentType), src.IDType)))
                .ForMember(dest => dest.SexName, opt => opt.MapFrom(src => EnumName.GetDescription(typeof(SexEnum), src.Sex)));

            CreateMap<ViewTimLifeClassStudent, TimeClassStudentResponse>();

            //2、转班明细列表
            CreateMap<ViewTimChangeClass, ChangeClassListResponse>();
            //3、教师代课
            CreateMap<ViewTeacherNoAttendLesson, TeacherClassTimetableResponse>();

            //4、补课周补课
            CreateMap<ViewReplenishWeek, ReplenishWeekListResponse>();

            //5、补课周补课添加备注
            CreateMap<ReplenishWeekModifyRemarkRequest, TblDatClassReplenishWeek>();

            //6、课程映射关系
            CreateMap<ViewTimLessonStudent, TimeLessonStudentResponse>();

            //7、补课课程映射关系
            CreateMap<ViewTimReplenishLessonStudent, TimeLessonStudentResponse>();
        }
    }
}
