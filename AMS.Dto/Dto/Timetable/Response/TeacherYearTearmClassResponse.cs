using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：补课周补课老师的年度、学期、班级名称、班级代码类
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-15</para>
    /// </summary>
    public class TeacherYearTearmClassResponse
    {
        /// <summary>
        /// 年度
        /// </summary>
        public List<int> YearList { get; set; }

        /// <summary>
        /// 学期
        /// </summary>
        public List<TermOutDto> TermList { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public List<TeacherCourseOutDto> TeacherCourseList { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public List<TeacherDatClassOutDto> TeacherDatClassList { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public List<ClassRoomOutDto> ClassRoomList { get; set; }

    }
}
