using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 班级上课时间表
    /// </summary>
    public partial class TblAutClassTime
    {
        /// <summary>
        /// 班级上课时间表主健(TblAutClassTime)
        /// </summary>
        public long AutClassTimeId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 上课时间Id
        /// </summary>
        public long ClassTimeId { get; set; }
        /// <summary>
        /// 所属审核
        /// </summary>
        public long AuditId { get; set; }
        /// <summary>
        /// 班级主健(TblDatClass)
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 上课时间段主健(TblDatSchoolTime)
        /// </summary>
        public long SchoolTimeId { get; set; }
        /// <summary>
        /// -1删除0不变1修改
        /// </summary>
        public int DataStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
