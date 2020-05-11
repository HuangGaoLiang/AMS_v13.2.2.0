using AMS.Dto;

namespace AMS.Service
{
    /// <summary>
    /// 描述：补课周补课抽象类
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-03-12</para>
    /// </summary>
    public abstract class AdjustLessonReplenishWeekProvider : ILessonProvider
    {
        /// <summary>
        /// 补课周补课抽象类构造函数
        /// </summary>
        protected AdjustLessonReplenishWeekProvider()
        {
        }

        /// <summary>
        /// 课次业务类型
        /// </summary>
        public int BusinessType => (int)LessonBusinessType.AdjustLessonReplenishWeek;
    }
}
