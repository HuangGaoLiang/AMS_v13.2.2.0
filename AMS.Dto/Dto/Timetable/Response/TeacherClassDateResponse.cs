namespace AMS.Dto
{
    /// <summary>
    /// 老师上课日期
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-11</para>
    /// </summary>
    public class TeacherClassDateResponse
    {
        /// <summary>
        /// 上课日期
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 标记 是否有学生未考勤
        /// </summary>
        public bool Mark { get; set; }
    }
}
