/*此代码由生成工具字段生成，生成时间2019/3/14 16:29:38 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 考勤确认表
    /// </summary>
    public partial class TblFinAttendanceConfirm
    {
        /// <summary>
        /// 主健(考勤确认表)课次ID
        /// </summary>
        public long AttendanceConfirmId { get; set; }

        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 老师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 确认的日期（0表示学期）
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 签字的图片地址
        /// </summary>
        public string TeacherSignUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

    }
}
