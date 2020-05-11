using System.ComponentModel;

namespace AMS.Dto.Enum
{
    /// <summary>
    /// K信老师扫码考勤响应状态码
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-06</para>
    /// </summary>
    public enum ScanCodeAttendStatusResponse
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        SUCCESS = 0,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        FAIL = 1,

        /// <summary>
        /// 多个选项
        /// </summary>
        [Description("多项")]
        MULTIPLE = 2,

        /// <summary>
        /// 系统警告
        /// </summary>
        [Description("系统警告")]
        SYSWARNING = 3,

        /// <summary>
        /// 温馨警告
        /// </summary>
        [Description("温馨警告")]
        WARNING = 4
    }
}
