using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 课程类型
    /// </summary>
    public enum LessonType : int
    {
        /// <summary>
        /// 常规课
        /// </summary>
        [Description("常规课")]
        RegularCourse = 1,
        /// <summary>
        /// 写生课
        /// </summary>
        [Description("写生课")]
        SketchCourse = 2,
    }
}
