using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 报名打印课程类型
    /// </summary>
    public enum EnrollOrderCouseType
    {

        /// <summary>
        /// 必修课程
        /// </summary>
        [Description("必修课")]
        Compulsory = 0,

        /// <summary>
        /// 选修课程
        /// </summary>
        [Description("选修课")]
        Elective = 1
    }
}
