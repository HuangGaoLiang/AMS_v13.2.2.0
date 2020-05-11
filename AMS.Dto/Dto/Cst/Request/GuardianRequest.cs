namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 监护人信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class GuardianRequest
    {
        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string GuardianName { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
