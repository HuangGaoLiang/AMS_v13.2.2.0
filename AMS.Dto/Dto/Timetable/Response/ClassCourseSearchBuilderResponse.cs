using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：查询的校区数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// <para></para>
    /// </summary>
    public class ClassCourseSearchSchoolResponse
    {
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public List<ClassCourseearchYearResponse> Years { get; set; }
    }
    /// <summary>
    /// 描述：年度学期
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class ClassCourseearchYearResponse
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 年份拥用的学期
        /// </summary>
        public List<ClassCourseSearchTermResponse> Terms { get; set; }
    }

    /// <summary>
    /// 描述：学期及学期下的课程
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class ClassCourseSearchTermResponse
    {
        /// <summary>
        /// 学期Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long TermId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 学期所属课程
        /// </summary>
        public List<ClassCourseResponse> TermCourse { get; set; }

    }

    /// <summary>
    /// 描述：教师及教师所上课程
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class ClassCourseSearchTeacherResponse
    {
        /// <summary>
        /// 教师师名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 教师ID
        /// </summary>
        public string TeacherNo { get; set; }

        /// <summary>
        /// 老师所要上课程
        /// </summary>
        public List<ClassCourseResponse> TeacherByCourse { get; set; }
    }
    /// <summary>
    /// 描述：课程信息
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class ClassCourseResponse
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }
        /// <summary>
        /// 班级中文名
        /// </summary>
        public string ClassCnName { get; set; }
    }
}
