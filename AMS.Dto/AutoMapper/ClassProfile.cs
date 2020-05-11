using System;
using System.Collections.Generic;
using System.Text;
using AMS.Storage.Models;
using AutoMapper;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class ClassProfile: Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ClassProfile()
        {
            //1、班级详情
            CreateMap<TblDatClass, ClassDetailResponse>();

            //2、班级
            CreateMap<TblDatClass, ClassListResponse>();
        }
    }
}
