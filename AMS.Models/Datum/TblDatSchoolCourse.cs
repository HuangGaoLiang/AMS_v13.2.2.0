using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 描述：校区授权课程
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2018-10-10 </para>
    /// </summary>
    public partial class TblDatSchoolCourse
    {
        /// <summary>
        /// 主健 校区授权课程
        /// </summary>
        public long SchoolCourseId { get; set; }
        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
