using System;

namespace AMS.Storage.Models
{
    /// <summary>
    ///  班级老师上课时间
    ///  <para>作    者：zhiwei.Tang</para>
    ///  <para>创建时间：2019-03-21</para>
    /// </summary>
    public class ViewClassTeacherDate
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 老师Id
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime ClassDate { get; set; }
    }
}
