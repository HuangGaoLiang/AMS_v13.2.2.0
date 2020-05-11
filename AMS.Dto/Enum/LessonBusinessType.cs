using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 课次业务类型
    /// </summary>
    public enum LessonBusinessType
    {
        /// <summary>
        /// 撤销排课
        /// </summary>
        [Description("撤销排课")]
        CancelMakeLess = -1,
        /// <summary>
        /// 报名排课
        /// </summary>
        [Description("报名排课")]
        EnrollMakeLesson = 1,
        /// <summary>
        /// 转班
        /// </summary>
        [Description("转班")]
        ChangeClass = 2,
        /// <summary>
        /// 补课
        /// </summary>
        [Description("补课")]
        RepairLesson = 3,

        /// <summary>
        /// 写生排课
        /// </summary>
        [Description("写生排课")]
        LifeClassMakeLesson = 4,

        /// <summary>
        /// 补课周补课
        /// </summary>
        [Description("补课周补课")]
        AdjustLessonReplenishWeek = 5,

        /// <summary>
        /// 调课
        /// </summary>
        [Description("调课")]
        AdjustLessonChange = 6,

        /// <summary>
        /// 插班补课
        /// </summary>
        //[Description("插班补课")]
        //ChangeSchool = 7,

        /// <summary>
        /// 老师代课
        /// </summary>
        [Description("老师代课")]
        AdjustLessonTeacher = 8,

        /// <summary>
        /// 全校上课日期调整
        /// </summary>
        [Description("全校上课日期调整")]
        AdjustLessonSchoolClassTime = 9,

        /// <summary>
        /// 班级上课时间调整
        /// </summary>
        [Description("班级上课时间调整")]
        AdjustLessonClassTime = 10
    }
}
