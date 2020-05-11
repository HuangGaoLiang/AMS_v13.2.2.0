using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Dto.AutoMapper;
using AMS.Storage.Context;
using AutoMapper;
using Jerrisoft.Platform.API;
using Jerrisoft.Platform.API.Filter;
using Jerrisoft.Platform.Config;
using Jerrisoft.Platform.Config.Configs;
using Jerrisoft.Platform.Exception;
using Jerrisoft.Platform.Log.Logs;
using Jerrisoft.Platform.Public.HttpContextCurrUser;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using YMM.HRS.SDK;

namespace Jerrisoft.Platform.TestWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private Dictionary<string, string> GetPermissionMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            return result;
        }

        // This method gets called 作     者 the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var mvcServices = services.AddMvc(option => option.Filters.Add(typeof(JerrisoftFilterAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //绑定其他API 
            InitBussinessLib(mvcServices);

            InitSwagger(services);


            AutoMapperProfileConfiguration.Configuration(); //DTO启动

            //配置文件
            new ConfClient().Start();

            //权限系统认证与授权
            services.AddIdentityClient()
                .AddJwtBearer(option =>
                {
                }).AddPermission(option =>
                {
                    option.PermissionMap = GetPermissionMap();
                });

            //任务调度密钥设置
            services.AddScheduleClient()
             .AddSchedule(option =>
             {
                 option.AccessSecret = AMS.Core.ClientConfigManager.AppsettingsConfig.JobAccessSecret;
             });
            Jerrisoft.Platform.Public.PlatformServiceCollection.Service = services;
            new AMS.API.Init.DefaultStartup().Run();

            CurrHttpContainer.SetInstance(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUser, CurrentUser>();
            #region Log4 日志配置
            LoggerRepositoryModel.LoggerRepository = LogManager.CreateRepository("JerrisoftCoreLog");
            XmlConfigurator.Configure(LoggerRepositoryModel.LoggerRepository, new FileInfo("log4net.config"));
            #endregion
        }

        // This method gets called 作     者 the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseJIdentity();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            //配置swagger相关
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/swagger1/swagger.json", "默认");
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "对外接口v1");
            });


            //映射错误日志文件夹为可访问
            string logfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logfile");
            if (!Directory.Exists(logfolder))
            {
                Directory.CreateDirectory(logfolder);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(logfolder),
                RequestPath = "/logfile",
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(logfolder),
                RequestPath = "/logfile"
            });

            app.UseCookiePolicy();

            //配置跨域相关
            app.UseCors(builder => builder.WithOrigins("*")
                .AllowAnyHeader().AllowAnyMethod());

            app.UseMvc();
        }

        //将其他业务API放到指定目录下，进行统一绑定
        private void InitBussinessLib(IMvcBuilder mvcBuilder)
        {
            var controllerPath = $"{AppDomain.CurrentDomain.BaseDirectory}Controller\\";
            if (!Directory.Exists(controllerPath))
            {
                return;
            }

            var pathFiles = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}Controller\\", "*.dll", SearchOption.AllDirectories);
            var pathFilesLength = pathFiles.Length;
            if (pathFilesLength <= 0)
            {
                return;
            }

            //需初始化的集合
            List<IStartupInit> startupInits = new List<IStartupInit>();
            List<Type> mapperTypes = new List<Type>();

            for (int i = 0; i < pathFilesLength; i++)
            {
                var assembly = Assembly.LoadFile(pathFiles[i]);
                mvcBuilder.AddApplicationPart(assembly);

                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.BaseType == typeof(Profile))
                    {
                        mapperTypes.Add(type);
                    }

                    Type starpUpInitType = typeof(IStartupInit);
                    if (starpUpInitType.IsAssignableFrom(type))
                    {
                        IStartupInit initObject = Activator.CreateInstance(type) as IStartupInit;
                        startupInits.Add(initObject);
                    }
                }
            }

            Mapper.Initialize(mapper => { mapperTypes.ForEach(type => mapper.AddProfile(type)); });
            startupInits = startupInits.OrderBy(x => x.Order).ToList();
            foreach (var s in startupInits)
            {
                if (s.IsAsync)
                {
                    Task.Run(() =>
                        s.Run()
                     );
                }
                else
                {
                    s.Run();
                }
            }

            Mapper.Initialize(mapper =>
            {
                mapper.AddProfile(typeof(NS1.Service.AutoMapper.DtoMapper));
                mapper.AddProfile(typeof(YMM.HRS.SDK.HrSystemMapper));
            });
        }

        /// <summary>
        /// 初始化Swagger
        /// </summary>
        /// <param name="services"></param>
        private void InitSwagger(IServiceCollection services)
        {

            //注册Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("swagger1", new Info
                {
                    Version = "swagger1",
                    Title = "杰人软件Swagger",
                    Description = "基于 NET CORE 2.1 进行开发。"
                });
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "杰人软件Swagger",
                    Description = "基于 NET CORE 2.1 进行开发。"
                });
                options.CustomSchemaIds(x => x.FullName);
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                options.AddSecurityDefinition("SSO-TOKEN", new ApiKeyScheme() { In = "header", Description = "统一登录的token(新)", Name = "SSO-TOKEN", Type = "apiKey" });
                //添加头部 token 
                options.AddSecurityDefinition("schoolNo", new ApiKeyScheme() { In = "header", Description = "校区编号", Name = "schoolNo", Type = "apiKey" });
                options.AddSecurityDefinition("year", new ApiKeyScheme() { In = "header", Description = "年度", Name = "year", Type = "apiKey" });
                options.AddSecurityDefinition("Jerrisoft_Token", new ApiKeyScheme() { In = "header", Description = "PC端Token", Name = "Jerrisoft_Token", Type = "apiKey" });
                options.AddSecurityDefinition("token", new ApiKeyScheme() { In = "header", Description = "APP端Token", Name = "token", Type = "apiKey" });
                options.AddSecurityDefinition("account", new ApiKeyScheme() { In = "header", Description = "APP端用户Id", Name = "account", Type = "apiKey" });
                options.AddSecurityDefinition("HSS-TOKEN", new ApiKeyScheme() { In = "header", Description = "家校互联token", Name = "HSS-TOKEN", Type = "apiKey" });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "SSO-TOKEN", Enumerable.Empty<string>() },
                    { "schoolNo", Enumerable.Empty<string>() },
                    { "year", Enumerable.Empty<string>() },
                    { "Jerrisoft_Token", Enumerable.Empty<string>() },
                    { "token", Enumerable.Empty<string>()  },
                    { "account", Enumerable.Empty<string>()  },
                    { "HSS-TOKEN", Enumerable.Empty<string>()  }
                });

                if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}Swagger"))
                {
                    Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}Swagger");
                }
                var pathFiles = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}Swagger\\", "*.xml", SearchOption.AllDirectories);
                var pathFilesLength = pathFiles.Length;
                if (pathFilesLength > 0)
                {
                    for (int i = 0; i < pathFilesLength; i++)
                    {
                        options.IncludeXmlComments(pathFiles[i]);
                    }
                }
            });
        }
    }
}
