using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core.Constants
{
    /// <summary>
    /// 课次处理过程常量值
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class LessonProcessConstants
    {
        public const string Remark = "撤销排课";

        public const string WeekRemark = "补课周补课";

        public const string WeekDeleteRemark = "删除补课周补课";

        public const string LessonTeacherRemark = "老师代课转出";

        public const string LessonSchoolClassTimeRemark = "全校上课日期调整";

        public const string LessonClassTimeRemark = "班级上课时间调整";

        public const string ChangeClassLessonReamrk = "转班时先销毁课次";

        public const string LeaveSchoolRemark = "休学";

        public const string LeaveClassLessonRemark = "报班退费课次销毁";
    }
}
