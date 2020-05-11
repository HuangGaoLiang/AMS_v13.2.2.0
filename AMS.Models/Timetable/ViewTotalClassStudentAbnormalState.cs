namespace AMS.Storage.Models
{
    /// <summary>
    /// 统计学生考勤异常
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    public class ViewTotalClassStudentAbnormalState
    {
        /// <summary>
        /// 统计数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 课次类型 1=常规课 2=写生课
        /// </summary>
        public int LessonType { get; set; }
    }
}
