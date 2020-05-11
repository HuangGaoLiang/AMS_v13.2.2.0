namespace AMS.Service
{
    /// <summary>
    /// 课次销毁基础信息
    /// </summary>
    public class LessonFinisherInfo
    {
        /// <summary>
        /// 课次ID
        /// </summary>
        public long LessonId { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;
    }
}
