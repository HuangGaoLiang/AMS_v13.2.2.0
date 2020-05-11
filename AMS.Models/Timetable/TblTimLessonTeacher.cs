using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 老师上课时间表
    /// </summary>
    public partial class TblTimLessonTeacher
    {
        /// <summary>
        /// 主健(老师上课时间表)
        /// </summary>
        public long LessonTeacherId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 老师ID
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 上课班级
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassTimeBegin { get; set; }
        /// <summary>
        /// 下课时间
        /// </summary>
        public DateTime ClassTimeEnd { get; set; }
        /// <summary>
        /// 考勤状态
        /// </summary>
        public int AttendStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
