using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 描述：班级上课时间课次调整提供者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public abstract class AdjustLessonClassTimeProvider : ILessonProvider
    {
        /// <summary>
        /// 描述：实例化一个班级上课时间课次调整提供者
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        protected AdjustLessonClassTimeProvider()
        {
        }
        /// <summary>
        /// 描述：业务类型
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        public abstract int BusinessType { get; set; }
    }
}
