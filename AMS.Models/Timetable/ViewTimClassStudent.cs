using Newtonsoft.Json;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 班级学生人数实体类
    /// </summary>
    public class ViewTimLifeClassStudent
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 学期Id
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int PersonNumber { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string WeekTimes { get; set; }
    }
}
