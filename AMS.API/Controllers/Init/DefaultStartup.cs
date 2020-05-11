using AMS.Core;
using AMS.Dto.AutoMapper;
using AMS.Service;
using AMS.Service.AuditFlow;
using Jerrisoft.Platform.API;
using Jerrisoft.Platform.Public;
using Jerrisoft.Platform.Public.HttpContextCurrUser;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.API.Init
{
    public class DefaultStartup : IStartupInit
    {
        public int Order => 1;

        public bool IsAsync => false;

        public void Run()
        {
            //学期审核监听
            TermAuditFactory termAuditService = new TermAuditFactory();
            //排课表监听
            TermCourseTimetableAuditFactory termCourseTimetableAuditService = new TermCourseTimetableAuditFactory();
            //赠与奖学金审核
            CouponRuleAuditFactory couponRuleAuditFactory = new CouponRuleAuditFactory();

            //任务调度访问配置

            //AutoMapperProfileConfiguration.Configuration();

            //if (PlatformServiceCollection.Service != null)
            //{
                //PlatformServiceCollection.Service
                //    .AddScheduleClient()
                //    .AddSchedule(option =>
                //    {
                //        option.AccessSecret = ClientConfigManager.AppsettingsConfig.JobAccessSecret;
                //});
            //}
        }
    }

    //public class A : Jerrisoft.Platform.Public.ITransient
    //{

    //}
}
