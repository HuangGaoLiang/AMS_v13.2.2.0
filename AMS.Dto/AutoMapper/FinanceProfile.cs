using AMS.Storage.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto.AutoMapper
{
    /// <summary>
    /// 财务相关的表与实体映射关系
    /// </summary>
    public class FinanceProfile : Profile
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FinanceProfile()
        {
            CreateMap<ViewFinOrderHandover, OrderHandoverListResponse>();//交接列表
        }
    }
}
