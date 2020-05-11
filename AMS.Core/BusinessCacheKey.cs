namespace AMS.Core
{
    /// <summary>
    /// 业务项目缓存KEY
    /// </summary>
    public static class BusinessCacheKey
    {
        //规则:项目名_模块_功能名

        /// <summary>
        /// 校区排课查询条件缓存KEY（已审）
        /// </summary>
        public static string ClassCourseSearchDataKey => $"{BusinessConfig.BussinessCode}_{ModelType.Timetable}_ClassCourseSearchData";

        /// <summary>
        /// 所有校区缓存KEY
        /// </summary>
        public static string SchoolDataKey => $"{BusinessConfig.BussinessCode}_{ModelType.Datum}_SchoolData";

        /// <summary>
        /// 城市缓存Key
        /// </summary>
        public static string CityDataKey => $"{BusinessConfig.BussinessCode}_{ModelType.Datum}_CityData";

        /// <summary>
        /// 所有校区老师缓存KEY
        /// </summary>
        public static string TeacherAllDataKey => $"{BusinessConfig.BussinessCode}_{ModelType.Datum}_TeacherData";

        /// <summary>
        /// 授权校区
        /// </summary>
        public static string RightShoolDataKey => $"{BusinessConfig.BussinessCode}_{ModelType.Timetable}_RightShoolData";

        /// <summary>
        /// 家校互联登陆短信验证码,缓存，保证在30分钟内发布同一短信。
        /// </summary>
        public static string HSS_SMS_KEY=> $"{BusinessConfig.BussinessCode}:{ModelType.Hss}:SMS";
    }
}
