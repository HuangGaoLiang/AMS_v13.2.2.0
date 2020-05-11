using AMS.Core.Constants;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退班课次结束者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-9</para>
    /// </summary>
    public class LeaveClassLessonFinisher : ILessonFinisher
    {
        private readonly TblOdrRefundOrder _tblOdrRefundOrder;        //退费订单主表对象
        private readonly TblOdrLeaveClassOrder _tblOdrLeaveClassOrder;//退班表对象    
        private readonly List<long> _refundEnllorCourseId;            //退费的课程Id集合
        private readonly UnitOfWork _unitOfWork;                      //工作单元事务

        /// <summary>
        /// 描述：实例化提供退班销毁课次的对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <param name="tblOdrRefundOrder">退费订单主表对象</param>
        /// <param name="tblOdrLeaveClassOrder">退班表对象</param>
        /// <param name="refundEnllorCourseId">退费的课程Id集合</param>
        /// <param name="unitOfWork">工作单元事务</param>
        public LeaveClassLessonFinisher(TblOdrRefundOrder tblOdrRefundOrder, TblOdrLeaveClassOrder tblOdrLeaveClassOrder, List<long> refundEnllorCourseId, UnitOfWork unitOfWork)
        {
            _tblOdrRefundOrder = tblOdrRefundOrder;
            _tblOdrLeaveClassOrder = tblOdrLeaveClassOrder;
            _refundEnllorCourseId = refundEnllorCourseId;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonFinish()
        {
        }

        /// <summary>
        /// 描述：获取退费要销毁的课次
        /// <para>作   者：瞿琦</para>
        /// <para>创建时间：2018-11-9</para>
        /// </summary>
        /// <returns>要销毁的课次信息集合</returns>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            //传入学生Id和休学日期,获取要销毁的课次Id
            var service = new StudentTimetableService(this._tblOdrRefundOrder.SchoolId, this._tblOdrRefundOrder.StudentId);
            var leaveList = service.GetLeaveSchoolLessonsList(_tblOdrLeaveClassOrder.StopClassDate, _unitOfWork).Where(x => _refundEnllorCourseId.Contains(x.EnrollOrderItemId));
            var result = leaveList.Select(x => new LessonFinisherInfo
            {
                LessonId = x.LessonId,
                BusinessId = this._tblOdrRefundOrder.RefundOrderId,
                BusinessType = (int)ProcessBusinessType.F_Refund,
                Remark = LessonProcessConstants.LeaveClassLessonRemark
            }).ToList();
            return result;
        }
    }
}
