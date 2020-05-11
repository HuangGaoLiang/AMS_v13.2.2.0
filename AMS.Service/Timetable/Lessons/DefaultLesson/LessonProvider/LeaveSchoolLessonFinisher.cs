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
    /// 描述：休学课次结束者
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class LeaveSchoolLessonFinisher : ILessonFinisher
    {
        private readonly TblOdrRefundOrder _tblOdrRefundOrder;                //退费订单信息
        private readonly TblOdrLeaveSchoolOrder _tblOdrLeaveSchoolOrder;      //休学信息信息
        private readonly UnitOfWork _unitOfWork;                              //事务
        private const string _remark = "休学";                                //备注

        /// <summary>
        /// 描述：实例化一个休学课次销毁者
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="tblOdrRefundOrder">退费订单信息</param>
        /// <param name="tblOdrLeaveSchoolOrder">休学订单信息</param>
        /// <param name="unitOfWork">工作单元事务</param>
        public LeaveSchoolLessonFinisher(TblOdrRefundOrder tblOdrRefundOrder, TblOdrLeaveSchoolOrder tblOdrLeaveSchoolOrder, UnitOfWork unitOfWork)
        {
            _tblOdrRefundOrder = tblOdrRefundOrder;
            _tblOdrLeaveSchoolOrder = tblOdrLeaveSchoolOrder;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 暂不实现
        /// </summary>
        public void AfterLessonFinish()
        {

        }


        /// <summary>
        /// 描述：获取要销毁的课次列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <returns>要销毁的课次Id集合</returns>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">无</exception>
        public List<LessonFinisherInfo> GetLessonFinisherInfo()
        {
            //传入学生Id和休学日期,获取要销毁的课次Id
            var service = new StudentTimetableService(this._tblOdrRefundOrder.SchoolId, this._tblOdrRefundOrder.StudentId);
            var leaveList = service.GetLeaveSchoolLessonsList(_tblOdrLeaveSchoolOrder.LeaveTime, _unitOfWork);
            var result = leaveList.Select(x => new LessonFinisherInfo
            {
                LessonId = x.LessonId,
                BusinessId = this._tblOdrRefundOrder.RefundOrderId,
                BusinessType = (int)ProcessBusinessType.F_LeaveSchool,   //没有枚举类型
                Remark = LessonProcessConstants.LeaveSchoolRemark,
            }).ToList();
            return result;
        }
    }
}
