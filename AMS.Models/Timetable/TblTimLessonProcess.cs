using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 课次变更记录表
    /// </summary>
    public partial class TblTimLessonProcess
    {
        /// <summary>
        /// 主健(课次变更记录表)
        /// </summary>
        public long LessonProcessId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 根源课次ID
        /// </summary>
        public long RootRawLessonId { get; set; }
        /// <summary>
        /// 源课次ID
        /// </summary>
        public long RawLessonId { get; set; }
        /// <summary>
        /// 课次ID
        /// </summary>
        public long LessonId { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public byte ProcessStatus { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
