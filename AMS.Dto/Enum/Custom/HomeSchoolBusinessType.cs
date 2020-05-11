using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto.Enum.Custom
{
    /// <summary>
    /// 描述：家校互联后台设置业务类型
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-13</para>
    /// </summary>
    public enum HomeSchoolBusinessType
    {
        /// <summary>
        /// 投诉与建议接收人
        /// </summary>
        [Description("投诉与建议接收人")]
        Feedback =1,
        /// <summary>
        /// 学生证正面
        /// </summary>
        [Description("学生证正面")]
        StudentCradUp =2,
        /// <summary>
        /// 学生证反面
        /// </summary>
        [Description("学生证反面")]
        StudentCradDown =3


    }
}
