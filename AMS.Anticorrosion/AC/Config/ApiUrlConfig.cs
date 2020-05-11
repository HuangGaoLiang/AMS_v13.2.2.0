using AMS.Core;

namespace AMS.Anticorrosion.AC
{
    /// <summary>
    /// 描    述: 学生信息更新至档案库
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>  
    public static class ApiUrlConfig
    {
        /// <summary>
        /// 档案库系统 http://archives.ymm.cn
        /// </summary>
        internal static string ACURL => ClientConfigManager.ApiGatewayConfig.ACUrl;

        /// <summary>
        /// 注册学生信息到档案库
        /// </summary>
        internal static string ACAddURL => $"{ACURL}api/ArtLibrary/1002";
    }
}
