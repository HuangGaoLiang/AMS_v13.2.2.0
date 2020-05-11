using AMS.Dto;

namespace AMS.Service
{
    public abstract class AdjustLessonChangeProvider : ILessonProvider
    {
 
        protected AdjustLessonChangeProvider()
        {
        }

        public int BusinessType => (int)LessonBusinessType.AdjustLessonChange;
    }
}
