/*此代码由生成工具字段生成，生成时间2018/10/27 16:52:22 */
using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生联系人表
    /// </summary>
    public partial class TblCstStudentContact
    {
        /// <summary>
        /// 主健(TblCstStudentContact)
        /// </summary>
        public long StudentContactId { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string GuardianName { get; set; }

        /// <summary>
        /// 与学生的关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
