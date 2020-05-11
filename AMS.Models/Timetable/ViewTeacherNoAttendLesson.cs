using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Models
{
    /// <summary>
    /// 描述：老师未上课课次信息
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-7</para>
    /// </summary>
    public class ViewTeacherNoAttendLesson
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }
        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string ClassTime { get; set; }
        ///// <summary>
        ///// 开始上课时间
        ///// </summary>
        //public string ClassBeginTime { get; set; }
        ///// <summary>
        ///// 上课结束时间
        ///// </summary>
        //public string ClassEndTime { get; set; }

    }
}
