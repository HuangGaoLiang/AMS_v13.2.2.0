using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 校区写生排课表
    /// </summary>
    public partial class TblTimLifeClass
    {
        /// <summary>
        /// 主健(校区写生排课表)
        /// </summary>
        public long LifeClassId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学期
        /// </summary>
        public long TermId { get; set; }
        /// <summary>
        /// 写生课代码
        /// </summary>
        public string LifeClassCode { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 写生地点
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 上课开始时间
        /// </summary>
        public DateTime? ClassBeginTime { get; set; }
        /// <summary>
        /// 上课结束时间
        /// </summary>
        public DateTime? ClassEndTime { get; set; }
        /// <summary>
        /// 消耗课次
        /// </summary>
        public int UseLessonCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
