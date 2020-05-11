using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 补课周补课视图类
    /// </summary>
    public class ViewReplenishWeek
    {
        /// <summary>
        /// 学生主键编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 缺课课次
        /// </summary>
        public int ClassNumber { get; set; }

        /// <summary>
        /// 补课周主键编号
        /// </summary>
        public long? ReplenishWeekId { get; set; }
        

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 学期编号
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 上课班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 老师编号
        /// </summary>
        public string TeacherId { get; set; }
    }
}
