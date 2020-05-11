using Jerrisoft.Platform.Config;
using Jerrisoft.Platform.Config.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Core
{
    /// <summary>
    /// 统一配置管理
    /// </summary>
    public  class ClientConfigManager //: ConfigManager
    {
        /// <summary>
        /// 不允许直接实例化
        /// </summary>
        protected ClientConfigManager()
        {

        }

        public const string APP_CODE = BusinessConfig.BussinessCode;

        /// <summary>
        /// 项目系统配置文件
        /// </summary>
        public static HssConfig HssConfig => ConfReader.Get<HssConfig>(APP_CODE, "hss.json"); 

        /// <summary>
        /// 网址访问配置
        /// </summary>
        public new static ApiGatewayConfig ApiGatewayConfig => ConfReader.Get<ApiGatewayConfig>(APP_CODE,"apigateway.json");

        /// <summary>
        /// appseetings
        /// </summary>
        public static AppsettingsConfig AppsettingsConfig => ConfReader.Get<AppsettingsConfig>(APP_CODE, "appsettings.json");
    }
    
}
