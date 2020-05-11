namespace AMS.Storage.Models
{
    /// <summary>
    /// 课程级别关联视图
    /// </summary>
    public class ViewCourseLevelMiddle
    {
        /// <summary>
        /// 课程级别关联中间件Id
        /// </summary>
        public long CourseLevelMiddleId { get; set; }

        /// <summary>
        /// 课程表主键Id
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 级别表主键Id
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 开始岁数
        /// </summary>
        public int BeginAge { get; set; }

        /// <summary>
        /// 结束岁数
        /// </summary>
        public int EndAge { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 课程级别代码
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// 课程级别中文名称
        /// </summary>
        public string LevelCnName { get; set; }

        /// <summary>
        /// 课程级别英文名称
        /// </summary>
        public string LevelEnName { get; set; }

        /// <summary>
        /// 级别是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
