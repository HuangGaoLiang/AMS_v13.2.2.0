using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core.Constants
{
    /// <summary>
    /// 扫码考勤常量配置
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-21</para>
    /// </summary>
    public class ScanCodeAttendConstants
    {
        public const string QrCodeInvalid = "无效二维码";

        public const string AttendError = "考勤时间设置错误";
        public const string AttendError1 = "未到考勤时间";
        public const string AttendError2 = "{0}同学在{1}完成考勤";

        public const string Unbound = "未绑定家校互联";

        public const string CourseError = "当前没有要上的课程";

        public const string SysError = "系统故障,请稍后再试";

        public const string Msg = "[点击补签到]";
    }
}
