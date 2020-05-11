using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 转班表
    /// </summary>
    public partial class TblTimChangeClass 
    {
        /// <summary>
        /// 主健(转班表)
        /// </summary>
        public long ChangeClassId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 所属报名订单课程
        /// </summary>
        public long EnrollOrderItemId { get; set; }
        /// <summary>
        /// 转出班级
        /// </summary>
        public long OutClassId { get; set; }
        /// <summary>
        /// 转入班级
        /// </summary>
        public long InClassId { get; set; }
        /// <summary>
        /// 转入日期
        /// </summary>
        public DateTime InDate { get; set; }
        /// <summary>
        /// 转出时间
        /// </summary>
        public DateTime OutDate { get; set; }
        /// <summary>
        /// 转入课次
        /// </summary>
        public int ClassTimes { get; set; }
        /// <summary>
        /// 转班原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
