using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 学期班级排课课表修改
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class EditClassScheduleRequest
    {
        /// <summary>
        /// 班级编号
        /// </summary>
        [Required]
        public long CourseId { get; set; }

        /// <summary>
        /// 课程等级编号
        /// </summary>
        [Required]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        [Required]
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课老师编号
        /// </summary>
        [Required]
        public string TeacherId { get; set; }

        /// <summary>
        /// 课次总数
        /// </summary>
        [Required]
        public int CourseNum { get; set; }

        /// <summary>
        /// 学位
        /// </summary>
        [Required]
        public int StudentsNum { get; set; }
    }
}
