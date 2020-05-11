using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 课程级别设置
    /// </summary>
    public partial class TblDatCourseLevel
    {
        /// <summary>
        /// 课程级别主健(TblDatCourseLevel)
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 课程级别代码
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string LevelCnName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string LevelEnName { get; set; }
        /// <summary>
        /// 停用/启用（0,：启用 1：禁用）
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
