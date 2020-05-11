using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 课程类别
    /// </summary>
    public enum CourseType
    {
        /// <summary>
        /// 必修课程
        /// </summary>
        [Description("必修课程")]
        Compulsory = 0,

        /// <summary>
        /// 选修课程
        /// </summary>
        [Description("选修课程")]
        Elective = 1
    }
}
