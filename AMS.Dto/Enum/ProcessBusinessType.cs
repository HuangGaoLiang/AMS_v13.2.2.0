/****************************************************************************\
所属系统：招生系统
所属模块：课表模块
创建时间：2019-03-15
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System.ComponentModel;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：课次业务类型（课次变更记录）
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public enum ProcessBusinessType
    {
        #region 课程创建 (C开头表示创建)
        /// <summary>
        /// 报名排课
        /// </summary>
        [Description("报名排课")]
        C_EnrollMakeLesson = 1,

        /// <summary>
        /// 转班
        /// </summary>
        [Description("转班")]
        C_ChangeClass = 2,

        /// <summary>
        /// 补课
        /// </summary>
        [Description("补课")]
        C_RepairLesson = 3,

        /// <summary>
        /// 写生排课
        /// </summary>
        [Description("写生排课")]
        C_LifeClassMakeLesson = 4,

        /// <summary>
        /// 补课周补课
        /// </summary>
        [Description("补课周补课")]
        C_AdjustLessonReplenishWeek = 5,

        /// <summary>
        /// 调课
        /// </summary>
        [Description("调课")]
        C_AdjustLessonChange = 6,

        /// <summary>
        /// 插班补课
        /// </summary>
        [Description("插班补课")]
        C_InsertClassRepairLesson = 7,

        /// <summary>
        /// 老师代课
        /// </summary>
        [Description("老师代课")]
        C_AdjustLessonTeacher = 8,

        /// <summary>
        /// 全校上课日期调整
        /// </summary>
        [Description("全校上课日期调整")]
        C_AdjustLessonSchoolClassTime = 9,

        /// <summary>
        /// 班级上课时间调整
        /// </summary>
        [Description("班级上课时间调整")]
        C_AdjustLessonClassTime = 10,
        #endregion

        #region 课程销毁 (F开头表示销毁)
        /// <summary>
        /// 报名作废
        /// </summary>
        [Description("报名作废")]
        F_EnrollMakeLesson = -1,

        /// <summary>
        /// 转班作废
        /// </summary>
        [Description("转班作废")]
        F_ChangeClass = -2,

        /// <summary>
        /// 补课转出
        /// </summary>
        [Description("补课转出")]
        F_RepairLesson = -3,

        /// <summary>
        /// 写生排课作废
        /// </summary>
        [Description("写生排课作废")]
        F_LifeClassMakeLesson = -4,

        /// <summary>
        /// 调课调出
        /// </summary>
        [Description("调课调出")]
        F_AdjustLessonChange = -6,

        /// <summary>
        /// 插班补课
        /// </summary>
        [Description("插班补课")]
        F_InsertClassRepairLesson = -7,

        /// <summary>
        /// 老师代课转出
        /// </summary>
        [Description("老师代课转出")]
        F_AdjustLessonTeacher = -8,

        /// <summary>
        /// 全校上课日期调整
        /// </summary>
        [Description("全校上课日期调整")]
        F_AdjustLessonSchoolClassTime = -9,

        /// <summary>
        /// 班级上课时间调整
        /// </summary>
        [Description("班级上课时间调整")]
        F_AdjustLessonClassTime = -10,

        /// <summary>
        /// 退费办理
        /// </summary>
        [Description("退费办理")]
        F_Refund = -20,

        /// <summary>
        /// 转校
        /// </summary>
        [Description("转校")]
        F_ChangeSchool = -21,

        /// <summary>
        /// 休学
        /// </summary>
        [Description("休学")]
        F_LeaveSchool = -22,
        #endregion
    }
}