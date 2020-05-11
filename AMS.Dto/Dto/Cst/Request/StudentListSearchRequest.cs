namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 学生列表查询类
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class StudentListSearchRequest
    {
        /// <summary>
        /// 学生在读状态:1在读 2休学 3流失
        /// </summary>
        public int StudySatus { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int LessonCount { get; set; }

        /// <summary>
        /// 学生信息 （姓名或监护人手机号码）
        /// </summary>
        public string KeyWord { get; set; }
    }
}
