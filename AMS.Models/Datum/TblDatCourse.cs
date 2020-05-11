using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 课程表
    /// </summary>
    public partial class TblDatCourse
    {
        /// <summary>
        /// 课程主健 课程表
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 课程代码
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// 课程类别 0:必修课程 1:选修课程
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 课程简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 课程中文名
        /// </summary>
        public string CourseCnName { get; set; }

        /// <summary>
        /// 课程英文名
        /// </summary>
        public string CourseEnName { get; set; }

        /// <summary>
        /// 班级中文名
        /// </summary>
        public string ClassCnName { get; set; }
        /// <summary>
        /// 班级英文名
        /// </summary>
        public string ClassEnName { get; set; }
        /// <summary>
        /// 是否禁用 0:启用 1:禁用
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
