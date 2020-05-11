using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using YMM.HRS.SDK;

namespace AMS.Dto.AutoMapper
{
    public class AutoMapperProfileConfiguration
    {
        public static void Configuration()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.AddProfile<DatumProfile>();
                mapper.AddProfile<TimetableProfile>();
                mapper.AddProfile<AuditFlowProfile>();


                mapper.AddProfile<CashProfile>();
                mapper.AddProfile<CstProfile>();
                mapper.AddProfile<DiscountProfile>();
                mapper.AddProfile<OrdersProfile>();
                mapper.AddProfile<FinanceProfile>();


                mapper.AddProfile<HrSystemMapper>();
            });
        }
    }
}
