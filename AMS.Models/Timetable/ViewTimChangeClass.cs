using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Models
{
    /// <summary>
    /// 描述：转班明细信息列表
    /// <para>作者:瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class ViewTimChangeClass 
    {
        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime EnrollTime { get; set; }
        /// <summary>
        /// 课程实收价
        /// </summary>
        public decimal TotalTradeAmount { get; set; }
        /// <summary>
        /// 报名课次
        /// </summary>
        public int EnrollClassTimes { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 学期类型Id
        /// </summary>
        public long TermTypeId { get; set; }
        /// <summary>
        /// 转班日期
        /// </summary>
        public DateTime ChangeClassSubmitTime { get; set; }

        /// <summary>
        /// 转出班级Id
        /// </summary>
        public long OutClassId { get; set; }
        /// <summary>
        /// 停课时间
        /// </summary>
        public DateTime OutDate { get; set; }
        /// <summary>
        /// 转班课次
        /// </summary>
        public int ChangeClassTimes { get; set; }
        /// <summary>
        /// 转入班级Id
        /// </summary>
        public long InClassId { get; set; }
        /// <summary>
        /// 转入班级时间
        /// </summary>
        public DateTime InDate { get; set; }
    }
}
