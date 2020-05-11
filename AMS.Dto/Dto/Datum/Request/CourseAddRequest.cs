using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 课程添加的数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseAddRequest
    {
        /// <summary>
        /// 课程类别
        /// </summary>
        [Required]
        public CourseType CourseType { get; set; }

        /// <summary>
        /// 课程代码
        /// </summary>
        [Required]
        [StringLength(10)]
        public string CourseCode { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ShortName { get; set; }

        /// <summary>
        /// 课程中文名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CourseCnName { get; set; }

        /// <summary>
        /// 课程英文名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CourseEnName { get; set; }

        /// <summary>
        /// 班级中文名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ClassCnName { get; set; }

        /// <summary>
        /// 班级英文名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ClassEnName { get; set; }

        /// <summary>
        /// 课程级别
        /// </summary>
        public List<CourseLevelMidAddRequest> CourseLevels { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [JsonIgnore]
        public long CourseId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        [JsonIgnore]
        public string CompanyId { get; set; }
    }

    /// <summary>
    /// 课程级别添加的数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class CourseLevelMidAddRequest
    {
        /// <summary>
        /// 课程等级Id
        /// </summary>
        [Required]
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 开始年龄
        /// </summary>
        [Required]
        public int SAge { get; set; }

        /// <summary>
        /// 结束年龄
        /// </summary>
        [Required]
        public int EAge { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [Required]
        public int Duration { get; set; }
    }
}
