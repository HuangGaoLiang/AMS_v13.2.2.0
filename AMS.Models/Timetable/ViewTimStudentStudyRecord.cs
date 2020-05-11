using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 学生学习记录
    /// </summary>
    public class ViewTimStudentStudyRecord
    {
        /// <summary>
        /// 报名明细Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 教师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 排课课次
        /// </summary>
        public int ArrangedClassTimes { get; set; }

        /// <summary>
        /// 已上课次
        /// </summary>
        public int AttendedClassTimes { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int RemainingClassTimes { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 学期类型Id
        /// </summary>
        public long TermTypeId { get; set; }

        /// <summary>
        /// 学期
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 上课教室
        /// </summary>
        public string RoomNo { get; set; }
    }
}
