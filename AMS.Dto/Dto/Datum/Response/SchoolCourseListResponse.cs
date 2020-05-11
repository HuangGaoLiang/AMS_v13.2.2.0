using System.Collections.Generic;
namespace AMS.Dto
{
    /// <summary>
    /// 描述：校区的授权课程
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary> 
    public class SchoolCourseListResponse
    {
        /// <summary>
        /// 校区id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 必修课程
        /// </summary>
        public List<string> RequiredCourse { get; set; }
        /// <summary>
        /// 选修课程
        /// </summary>
        public List<string> ElectiveCourse { get; set; }


    }
}
