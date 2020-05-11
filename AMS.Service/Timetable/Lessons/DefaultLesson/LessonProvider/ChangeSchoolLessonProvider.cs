using AMS.Dto;

namespace AMS.Service
{
    public abstract class ChangeSchoolLessonProvider : ILessonProvider
    {
        protected readonly long _refundOrderId;      //退费订单Id
        protected ChangeSchoolLessonProvider(long refundOrderId)
        {
            this._refundOrderId = refundOrderId;
        }

        /// <summary>
        /// 业务类型
        /// 转校取消还原课次类型为排课
        /// </summary>
        public int BusinessType => (int)LessonBusinessType.EnrollMakeLesson;
    }
}
