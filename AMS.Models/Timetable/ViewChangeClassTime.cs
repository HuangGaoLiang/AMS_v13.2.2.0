using System;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 上课时间变化统计
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-22</para>
    /// </summary>
    public class ViewChangeClassTime
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long  ClassId { get; set; }

        /// <summary>
        /// 新上课日期
        /// </summary>
        public DateTime NewClassDate { get; set; }

        /// <summary>
        /// 新上课开始时间
        /// </summary>
        public string NewClassBeginTime { get; set; }

        /// <summary>
        /// 新上课结束时间
        /// </summary>
        public string NewClassEndTime { get; set; }

        /// <summary>
        /// 旧上课日期
        /// </summary>
        public DateTime OldClassDate { get; set; }

        /// <summary>
        /// 旧上课开始时间
        /// </summary>
        public string OldClassBeginTime { get; set; }

        /// <summary>
        /// 旧上课结束时间
        /// </summary>
        public string OldClassEndTime { get; set; }
    }
}
