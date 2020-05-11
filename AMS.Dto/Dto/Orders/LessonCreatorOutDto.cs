namespace AMS.Dto
{
    /// <summary>
    /// 返回课次创建结果
    /// </summary>
    public class LessonCreatorOutDto
    {
        /// <summary>
        /// 所属报名项
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }
}
