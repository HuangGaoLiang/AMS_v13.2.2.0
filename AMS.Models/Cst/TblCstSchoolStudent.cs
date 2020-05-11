/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生校区归属表
    /// </summary>
    public partial class TblCstSchoolStudent
    {
        /// <summary>
        /// 主健(TblCstSchoolStudent)
        /// </summary>
        public long SchoolStudentId { get; set; }

        /// <summary>
        /// 归属公司
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 归属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学生状态（1在读2休学3流失）
        /// </summary>
        public int StudyStatus { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int RemindClassTimes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
