using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 添加课程jibie
    /// </summary>
    public class CourseLevelAddRequest
    {
        /// <summary>
        /// 课程级别主健(TblDatCourseLevel)
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        [JsonIgnore]
        public string CompanyId { get; set; }

        /// <summary>
        /// 课程级别代码
        /// </summary>
        [Required]
        public string LevelCode { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        [Required]
        public string LevelCnName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [Required]
        public string LevelEnName { get; set; }
    }


}
