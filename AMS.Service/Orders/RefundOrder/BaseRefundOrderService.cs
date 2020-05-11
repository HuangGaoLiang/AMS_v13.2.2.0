using AMS.Dto;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退费订单基类
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public abstract class BaseRefundOrderService : BaseOrderService
    {

        protected string _schoolId;                                                                     //校区ID      
        protected long _studentId;                                                                      //学生ID
        protected readonly Lazy<TblOdrRefundOrderRepository> _tblOdrRefundOrderRepository;              //退费订单
        protected readonly Lazy<TblOdrRefundOrdeEnrollRepository> _tblOdrRefundOrdeEnrollRepository;    //退费订单课程明细
        protected readonly Lazy<TblOdrRefundOrderCostRepository> _tblOdrRefundOrderCost;                //退费订单-其他费用
        protected readonly Lazy<TblOdrRefundPayRepository> _tblOdrRefundPay;                            //退费订单-支付信息

        /// <summary>
        /// 描述：退费订单基类实例化
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        protected BaseRefundOrderService(string schoolId, long studentId)
        {
            this._schoolId = schoolId;
            this._studentId = studentId;
            _tblOdrRefundOrderRepository = new Lazy<TblOdrRefundOrderRepository>();
            _tblOdrRefundOrdeEnrollRepository = new Lazy<TblOdrRefundOrdeEnrollRepository>();
            _tblOdrRefundOrderCost = new Lazy<TblOdrRefundOrderCostRepository>();
            _tblOdrRefundPay = new Lazy<TblOdrRefundPayRepository>();
        }

        /// <summary>
        /// 描述：办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        public abstract void Transact(IRefundOrderTransactRequest iRequest, string companyId);


        /// <summary>
        /// 描述：办理前获取要办理的数据详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        public abstract IRefundOrderTransactDetailReponse GetTransactDetail(IRefundOrderTransacDetailtRequest iRequest, string companyId);


        /// <summary>
        /// 获取学生课次费用明细（已排课+未排课）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="leaveTime">休学日期</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>报名课次费用明细</returns>
        /// <exception cref="AMS.Core.BussinessException">无</exception>
        internal List<RefundOrderTransactDetailListResponse> GetCourseLessonList(DateTime leaveTime, string companyId)
        {
            //获取所有的课程信息
            var courseList = CourseService.GetAllAsync().Result;
            //获取所有的学期类型
            var termTypeList = new TermTypeService().GetAll();
            //获取所有的课程级别集合
            var courseLevelList = new CourseLevelService(companyId).GetList().Result;

            var enrollOrderTrade = new EnrollOrderService(this._schoolId);
            //已排课的信息
            var studentTimetableService = new StudentTimetableService(this._schoolId, this._studentId);
            var hasLessonList = studentTimetableService.GetLeaveSchoolLessonCount(leaveTime);  //获取休学课次信息

            var enroOrderList = new List<RefundOrderTransactDetailListResponse>();

            var enroOrderItemList = enrollOrderTrade.GetStudentEnroOrderItem(this._studentId, leaveTime);
            foreach (var item in enroOrderItemList)
            {
                var lessonArray = hasLessonList.FirstOrDefault(x => x.EnrollOrderItemId == item.EnrollOrderItemId);
                var lessonCount = lessonArray?.Count ?? 0;   //休学课次
                //上课课次
                var haveClassLesson = item.ClassTimes - (item.ClassTimes - item.ClassTimesUse) - lessonCount;   //上课课次=报名课次-未排课次-休学课次
                var refundNum = item.PayAmount - (haveClassLesson * (item.TuitionFee + item.MaterialFee));
                var refundAmount = refundNum > 0 ? refundNum : 0;             //退费金额=实收金额-扣除金额  退费金额为负数时等于0

                var entity = new RefundOrderTransactDetailListResponse
                {
                    Year = item.Year,
                    TermTypeId = item.TermTypeId,
                    TermTypeName = termTypeList.FirstOrDefault(k => k.TermTypeId == item.TermTypeId)?.TermTypeName,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    CourseId = item.CourseId,
                    CourseName = courseList.FirstOrDefault(k => k.CourseId == item.CourseId)?.CourseCnName,
                    CourseLevelId = item.CourseLevelId,
                    CourseLevelName = courseLevelList.FirstOrDefault(k => k.CourseLevelId == item.CourseLevelId)?.LevelCnName,
                    ClassTimes = item.ClassTimes,
                    PaidAmount = item.PayAmount,
                    HaveClassLesson = haveClassLesson,                        //上课课次=报名课次-休学课次-未排课课次
                    DeductAmount = haveClassLesson * (item.TuitionFee + item.MaterialFee),  //扣除费用=上课课次*原课次单价
                    LeaveSchoolLessons = lessonCount,           //排课部分休学课次
                    RefundAmount = refundAmount,            ////退费金额=实收金额-扣除金额  退费金额为负数时等于0  没有排课时退费金额=实收金额
                    TuitionFee = item.TuitionFee,
                    MaterialFee = item.MaterialFee,
                    DiscountFee = item.DiscountFee,
                    Status = item.Status
                };
                enroOrderList.Add(entity);
            }

            return enroOrderList;
        }

    }
}
