using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 描述：老师代课提供者
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-8</para>
    /// </summary>
    public abstract class AdjustLessonTeacherProvider : ILessonProvider
    {
        /// <summary>
        /// 描述：实例化一个老师代课课次提供者
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-8</para>
        /// </summary>
        protected AdjustLessonTeacherProvider()
        {
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public abstract int BusinessType { get; set; }
    }
}
