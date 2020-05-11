using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 班级
    /// </summary>
    public partial class TblDatClass
    {
        /// <summary>
        /// 主键 班级表
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 学期表主健
        /// </summary>
        public long TermId { get; set; }
        /// <summary>
        /// 教室表主健
        /// </summary>
        public long RoomCourseId { get; set; }
        /// <summary>
        /// 教室Id
        /// </summary>
        public long ClassRoomId { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public long CourseId { get; set; }
        /// <summary>
        /// 课程等级
        /// </summary>
        public long CourseLeveId { get; set; }
        /// <summary>
        /// 上课老师编号
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 课次 
        /// </summary>
        public int CourseNum { get; set; }
        /// <summary>
        /// 学生数量/学位
        /// </summary>
        public int StudentsNum { get; set; }

        /// <summary>
        /// 1常规班级 2补课周班级
        /// </summary>
        public int ClassType { get; set; }
        
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
