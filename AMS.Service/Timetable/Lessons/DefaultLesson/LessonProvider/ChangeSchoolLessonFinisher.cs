using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 转校课次结束者
    /// </summary>
    public class ChangeSchoolLessonFinisher : ILessonFinisher, ILessonProvider
    {
        /// <summary>
        /// 退费订单Id
        /// </summary>
        private readonly long _refundOrderId;

        /// <summary>
        /// 转校订单仓储
        /// </summary>
        private readonly ViewChangeSchooolOrderRepository _viewChangeSchoolOrderRepository;

        /// <summary>
        /// 转校的课程明细仓储
        /// </summary>
        private readonly TblOdrRefundOrdeEnrollRepository _refundOrdeEnrollRepository;

        private readonly TblTimLessonRepository _lessonRepository;

        /// <summary>
        /// 根据转校订单获取一个课次销毁对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="refundOrderId">转校订单ID</param>
        /// <param name="unitOfWork">工作单元</param>
        public ChangeSchoolLessonFinisher(long refundOrderId, UnitOfWork unitOfWork)
        {
            this._refundOrderId = refundOrderId;

            _viewChangeSchoolOrderRepository =
                unitOfWork.GetCustomRepository<ViewChangeSchooolOrderRepository, ViewChangeSchooolOrder>();

            _refundOrdeEnrollRepository =
                 unitOfWork.GetCustomRepository<TblOdrRefundOrdeEnrollRepository, TblOdrRefundOrdeEnroll>();

            _lessonRepository =
                unitOfWork.GetCustomRepository<TblTimLessonRepository, TblTimLesson>();
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType => (int)ProcessBusinessType.F_ChangeSchool;

        /// <summary>
        /// 课次销毁之后的回调函数
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        public void AfterLessonFinish()
        {
        }

        /// <summary>
        /// 获取销毁的课次信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <returns>销毁的课次信息集合</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            List<LessonFinisherInfo> res = new List<LessonFinisherInfo>();

            //1.获取转校订单
            var csOrder = _viewChangeSchoolOrderRepository.GetChangeSchooolOrder(_refundOrderId);
            if (csOrder == null)
            {
                return res;
            }

            //2.获取转校的课程明细
            List<TblOdrRefundOrdeEnroll> refundOrdeEnrolls = _refundOrdeEnrollRepository.GetRefundOrderByOrderEnroll(_refundOrderId);
            List<long> enrollOrderItemIdList = refundOrdeEnrolls.Select(x => x.EnrollOrderItemId).ToList();
            if (enrollOrderItemIdList.Count == 0)
            {
                return res;
            }

            //3.获取课程明细的课次
            List<TblTimLesson> lessonList = _lessonRepository.GetLessonsListByEnrollOrderItemId(enrollOrderItemIdList);
            lessonList = lessonList.Where(x => x.ClassDate >= csOrder.OutDate).ToList(); //处理转校时间
            if (lessonList.Count == 0)
            {
                return res;
            }

            //4.转换销毁课次信息
            res = lessonList.Select(x => new LessonFinisherInfo
            {
                LessonId = x.LessonId,
                BusinessId = _refundOrderId,
                BusinessType = this.BusinessType
            })
            .ToList();

            return res;
        }
    }
}
