using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AMS.Storage.Repository.Orders;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退费订单－休学
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-7</para>
    /// </summary>
    public class LeaveSchoolOrderService : BaseRefundOrderService
    {
        protected override string OrderNoTag => "X";                                                //休学订单前缀        
        private readonly Lazy<TblOdrLeaveSchoolOrderRepository> _tblOdrLeaveSchoolOrderRepository;  //退费订单--休学 

        /// <summary>
        /// 描述：实例化指定校区的休学对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public LeaveSchoolOrderService(string schoolId) : this(schoolId, 0)
        {

        }
        /// <summary>
        /// 描述：实例化一个学生要在指定校区的休学对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public LeaveSchoolOrderService(string schoolId, long studentId) : base(schoolId, studentId)
        {
            _tblOdrLeaveSchoolOrderRepository = new Lazy<TblOdrLeaveSchoolOrderRepository>();
        }

        #region GetOrderDetail 获取订单详情
        /// <summary>
        /// 描述：获取订单详情
        /// <para>作  者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单主键Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>休学订单详情</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1,异常描述：系统不存在该退费订单
        /// 异常ID：10,异常描述：系统不存在该休学信息
        /// </exception>
        public override IOrderDetailResponse GetOrderDetail(long refundOrderId, string companyId)
        {
            var result = new LeaveSchoolOrderDetailResponse();

            var refundOrder = base._tblOdrRefundOrderRepository.Value.GetRefundOrderIdByRefundOrder(refundOrderId);
            if (refundOrder == null)
            {
                throw new BussinessException((byte)ModelType.Order, 1);
            }
            var leaveShoolOrder = _tblOdrLeaveSchoolOrderRepository.Value.GetLeaveSchoolIdByOrder(refundOrderId);
            if (leaveShoolOrder == null)
            {
                throw new BussinessException((byte)ModelType.Order, 10);
            }
            //获取休学的附件
            var attchmentService = new AttchmentService(base._schoolId);
            var attchmentList = attchmentService.GetAttchList(refundOrderId, AttchmentType.LEAVE_SCHOOL_APPLY);

            //休学费用明细
            var leaveSchoolOrderDetailList = LeaveSchoolOrderService.GetRefundCourseAmountDetail(refundOrderId, companyId);

            result.OrderNo = refundOrder.OrderNo;
            result.CreateTime = refundOrder.CreateTime;
            result.CreatorName = refundOrder.CreatorName;
            result.LeaveTime = leaveShoolOrder.LeaveTime;
            result.ResumeTime = leaveShoolOrder.ResumeTime;
            result.Reason = leaveShoolOrder.Reason;
            result.Remark = leaveShoolOrder.Remark;
            result.AttchmentUrl = attchmentList;
            result.LeaveSchoolOrderDetailList = leaveSchoolOrderDetailList;

            return result;
        }
        #endregion

        #region GetTransactDetail  根据休学日期获取当前学生费用明细
        /// <summary>
        /// 描述：根据休学日期获取当前学生费用明细
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="iRequest">休学开始日期</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>休学课次的退费课程订单详情</returns>
        public override IRefundOrderTransactDetailReponse GetTransactDetail(IRefundOrderTransacDetailtRequest iRequest, string companyId)
        {
            //请求参数
            var request = iRequest as LeaveClassOrderTransactDetailRequest;
            //响应信息
            var respon = new LeaveSchoolOrderTransactDetailResponse
            {
                LeaveSchoolOrderDetailList = base.GetCourseLessonList(request.LeaveTime, companyId).Where(x => x.RefundAmount > 0).ToList()   //页面只显示退费金额大于0的数据
            };
            return respon;
        }
        #endregion

        #region Transact 休学办理
        /// <summary>
        /// 描述：休学办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-7</para>
        /// </summary>
        /// <param name="iRequest">休学的相关数据</param>
        /// <param name="companyId">公司编号</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：8,异常描述：没有要休学的课程
        /// </exception>
        public override void Transact(IRefundOrderTransactRequest iRequest, string companyId)   //
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, base._schoolId, base._studentId.ToString()))
            {
                var request = iRequest as LeaveSchoolOrderAddRequest;
                //获取学生课次费用明细
                var courseLessonList = base.GetCourseLessonList(request.LeaveTime, companyId);

                if (!courseLessonList.Any()) //如果订单课程已休学则获取不到学生课次费用明细，则抛出异常
                {
                    throw new BussinessException(ModelType.Order, 8);
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        //1.写入退费订单主表                      --休学订单表
                        var refundOrder = AddOdrRefundOrder(request, courseLessonList, unitOfWork);
                        //2.写入退费订单课程明细
                        AddOdrRefundOrdeEnroll(courseLessonList, refundOrder, unitOfWork);
                        //3.写入休学表详细信息表
                        var leaveSchoolOrder = AddOdrLeaveSchoolOrder(request, courseLessonList, refundOrder, unitOfWork);
                        //4.添加附件
                        AddAttachment(request, leaveSchoolOrder, unitOfWork);
                        //5、课次调用   
                        LeaveSchoolLessonFinisher leaveSchoolFinisher = new LeaveSchoolLessonFinisher(refundOrder, leaveSchoolOrder, unitOfWork);
                        LessonService lessonService = new LessonService(unitOfWork);
                        lessonService.Finish(leaveSchoolFinisher);//课次销毁
                        //6、资金调用
                        LeaveSchoolOrderTrade orderTrade = new LeaveSchoolOrderTrade(refundOrder);
                        TradeService trade = new TradeService(orderTrade, unitOfWork);
                        trade.Trade();
                        //7、修改课程明细表为休学状态
                        EnrollOrderService service = new EnrollOrderService(this._schoolId);
                        var enrollOrderItemIdList = courseLessonList.Select(x => x.EnrollOrderItemId).ToList();
                        service.ChangeToLeaveSchool(enrollOrderItemIdList, unitOfWork).Wait();
                        //8.修改学生状态
                        var studentService = new StudentService(this._schoolId);
                        studentService.UpdateStudentStatusById(this._studentId, unitOfWork, StudyStatus.Suspension);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region GetOrderList 获取休学列表
        /// <summary>
        /// 描述：获取休学列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-6</para>
        /// </summary>
        /// <param name="iRequest">列表筛选条件</param>
        /// <returns>休学明细列表</returns>
        public override LeaveSchoolOrderListResponse GetOrderList(IOrderListSearchRequest iRequest)
        {
            var leaveSchoolListRespon = new LeaveSchoolOrderListResponse();
            var searcher = iRequest as LeaveSchoolOrderListSearchRequest;
            //1.实例化休学订单仓储
            var viewLeaveSchoolOrderRepository = new ViewLeaveSchoolOrderRepository();
            //2.获取休学列表
            var leaveSchoolQuery = viewLeaveSchoolOrderRepository.GetLeaveSchoolOrderList(_schoolId, searcher);
            var leaveSchoolList = AutoMapper.Mapper.Map<List<LeaveSchoolOrderInfoResponse>>(leaveSchoolQuery.Data);
            var leaveSchoolInfoList = new PageResult<LeaveSchoolOrderInfoResponse>
            {
                CurrentPage = leaveSchoolQuery.CurrentPage,
                Data = leaveSchoolList,
                PageSize = leaveSchoolQuery.PageSize,
                TotalData = leaveSchoolQuery.TotalData,
            };
            //获取休学人数
            leaveSchoolListRespon.LeaveSchoolCount = viewLeaveSchoolOrderRepository.QueryLeaveSchoolCount(_schoolId, searcher);
            //获取休学退费总金额
            leaveSchoolListRespon.LeaveSchoolAmount = viewLeaveSchoolOrderRepository.QueryLeaveSchoolAmount(_schoolId, searcher);
            //获取休学列表
            leaveSchoolListRespon.LeaveSchoolOrderList = leaveSchoolInfoList;
           

            return leaveSchoolListRespon;
        }
        #endregion

        #region AddOdrRefundOrder 写入退费订单主表
        /// <summary>
        /// 描述：写入退费订单主表
        /// <para>作    者：瞿  琦</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="request">要写入的休学数据</param>
        /// <param name="courseLessonList">休学课程明细</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>休学订单详情</returns>
        private TblOdrRefundOrder AddOdrRefundOrder(LeaveSchoolOrderAddRequest request, List<RefundOrderTransactDetailListResponse> courseLessonList, UnitOfWork unitOfWork)
        {
            var refundOrderId = IdGenerator.NextId();
            var refundOrder = new TblOdrRefundOrder    //1.退费订单表
            {
                RefundOrderId = refundOrderId,
                SchoolId = this._schoolId,
                StudentId = this._studentId,
                OrderNo = base.CreateOrderNo(refundOrderId),
                OrderType = (int)OrderTradeType.LeaveSchoolOrder,
                TotalDeductAmount = courseLessonList.Sum(x => x.DeductAmount),
                Amount = courseLessonList.Sum(x => x.RefundAmount),
                OrderStatus = (int)OrderStatus.Paid,
                CancelUserId = string.Empty,
                CancelDate = null,
                CancelRemark = string.Empty,
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName,
                CreateTime = DateTime.Now
            };
            var tblOdrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();
            tblOdrRefundOrderRepository.Add(refundOrder);
            return refundOrder;
        }
        #endregion

        #region AddOdrRefundOrdeEnroll 写入退费订单课程明细
        /// <summary>
        /// 描述:写入退费订单课程明细
        /// <para>作    者：瞿  琦</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="courseLessonList">休学课程明细</param>
        /// <param name="refundOrder">退费订单信息</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>无</returns>
        private void AddOdrRefundOrdeEnroll(List<RefundOrderTransactDetailListResponse> courseLessonList, TblOdrRefundOrder refundOrder, UnitOfWork unitOfWork)
        {
            var refundOrdeEnrollList = new List<TblOdrRefundOrdeEnroll>();
            foreach (var item in courseLessonList)
            {
                var refundOrdeEnroll = new TblOdrRefundOrdeEnroll()    //2.退费订单课程明细
                {
                    RefundOrderEnrollId = IdGenerator.NextId(),
                    SchoolId = base._schoolId,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    RefundOrderId = refundOrder.RefundOrderId,
                    LessonCount = item.LeaveSchoolLessons,
                    UseLessonCount = item.HaveClassLesson,
                    LessonPrice = item.TuitionFee + item.MaterialFee,
                    Amount = item.RefundAmount,
                    CreateTime = DateTime.Now
                };
                refundOrdeEnrollList.Add(refundOrdeEnroll);
            }
            var tblOdrRefundOrdeEnrollRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrdeEnrollRepository, TblOdrRefundOrdeEnroll>();
            tblOdrRefundOrdeEnrollRepository.Add(refundOrdeEnrollList);
        }
        #endregion

        #region AddOdrLeaveSchoolOrder 写入休学表详细信息表
        /// <summary>
        /// 描述:写入休学表详细信息表 
        /// <para>作    者：瞿  琦</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="request">要写入的休学数据</param>
        /// <param name="courseLessonList">休学课程明细</param>
        /// <param name="refundOrder">退费订单信息</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>休学订单详情</returns>

        private TblOdrLeaveSchoolOrder AddOdrLeaveSchoolOrder(LeaveSchoolOrderAddRequest request, List<RefundOrderTransactDetailListResponse> courseLessonList, TblOdrRefundOrder refundOrder, UnitOfWork unitOfWork)
        {
            var leaveSchoolOrder = new TblOdrLeaveSchoolOrder
            {
                RefundOrderId = refundOrder.RefundOrderId,   // refundOrderId,
                SchoolId = this._schoolId,
                LeaveTime = request.LeaveTime,
                ResumeTime = request.ResumeTime,
                TotalRefundLessonCount = courseLessonList.Sum(x => x.LeaveSchoolLessons),
                TotalUseLessonCount = courseLessonList.Sum(x => x.HaveClassLesson),
                Remark = request.Remark,
                Reason = request.Reason
            };
            var tblOdrLeaveSchoolOrderRepository = unitOfWork.GetCustomRepository<TblOdrLeaveSchoolOrderRepository, TblOdrLeaveSchoolOrder>();
            tblOdrLeaveSchoolOrderRepository.Add(leaveSchoolOrder);
            return leaveSchoolOrder;
        }
        #endregion

        #region AddAttachment 添加附件
        /// <summary>
        /// 描述:添加附件
        /// <para>作    者：瞿  琦</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="request">要写入的休学数据</param>
        /// <param name="leaveSchoolOrder">休学订单详情</param>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>无</returns>

        private void AddAttachment(LeaveSchoolOrderAddRequest request, TblOdrLeaveSchoolOrder leaveSchoolOrder, UnitOfWork unitOfWork)
        {
            //添加上传申请表附件到数据库
            foreach (var item in request.AttachmentUrlList)
            {
                item.AttchmentType = AttchmentType.LEAVE_SCHOOL_APPLY;
                item.BusinessId = leaveSchoolOrder.RefundOrderId;
            }
            var attchmentService = new AttchmentService(base._schoolId, unitOfWork);
            attchmentService.Add(request.AttachmentUrlList);  //添加附件
        }
        #endregion

        #region GetRefundCourseAmountDetail  根据退费订单的Id获取退费订单的课程明细
        /// <summary>
        /// 描述:根据退费订单的Id获取退费订单的课程明细
        /// <para>作    者：瞿  琦</para>
        /// <para>创建时间：2018-12-20</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单表Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>休学订单课程明细</returns>
        internal static List<RefundOrderTransactDetailListResponse> GetRefundCourseAmountDetail(long refundOrderId, string companyId)
        {
            //获取所有的课程信息
            var courseList = CourseService.GetAllAsync().Result;
            //获取所有的学期类型
            var termTypeList = new TermTypeService().GetAll();
            //获取所有的课程级别集合
            var courseLevelList = new CourseLevelService(companyId).GetList().Result;

            var refundCourseList = new TblOdrRefundOrdeEnrollRepository().GetRefundOrderByOrderEnroll(refundOrderId);
            var leaveSchoolCourseIdList = refundCourseList.Select(x => x.EnrollOrderItemId);

            var courseOrderDetailList = EnrollOrderService.GetEnrollOrderItemIdByEnroOrderList(leaveSchoolCourseIdList);
            var refundCourseDetail = new List<RefundOrderTransactDetailListResponse>();
            foreach (var item in courseOrderDetailList)
            {

                var refundCourseInfo = refundCourseList.FirstOrDefault(k => k.EnrollOrderItemId == item.EnrollOrderItemId);
                //获取上课课次
                var haveClassLesson = refundCourseInfo?.UseLessonCount ?? 0;
                //获取扣除费用
                var deductAmount = haveClassLesson * (item.MaterialFee + item.TuitionFee);
                //获取休学/退班课次
                var leaveSchoolLessons = refundCourseInfo?.LessonCount ?? 0;
                //获取退费金额
                var refundAmount = refundCourseInfo?.Amount ?? 0;

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
                    HaveClassLesson = haveClassLesson,   //上课课次
                    DeductAmount = deductAmount,         //扣除费用
                    LeaveSchoolLessons = leaveSchoolLessons,   //休学/退班课次
                    RefundAmount = refundAmount,            //退费金额
                    TuitionFee = item.TuitionFee,
                    MaterialFee = item.MaterialFee,
                    DiscountFee = item.DiscountFee,
                };
                refundCourseDetail.Add(entity);
            }

            return refundCourseDetail;
        }
        #endregion

        #region GetLeaveSchoolList 根据校区和学生Id获取休学信息
        /// <summary>
        /// 描述:根据校区和学生Id获取休学信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-12-5</para>
        /// </summary>
        /// <param name="unitOfWork">工作单元事务</param>
        /// <returns>休学订单详情</returns>

        internal TblOdrLeaveSchoolOrder GetLeaveSchoolList(UnitOfWork unitOfWork = null)
        {
            TblOdrRefundOrderRepository tblOdrRefundOrderRepository = null;
            TblOdrLeaveSchoolOrderRepository tblOdrLeaveSchoolOrderRepository = null;
            if (unitOfWork != null)
            {
                tblOdrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();
                tblOdrLeaveSchoolOrderRepository = unitOfWork.GetCustomRepository<TblOdrLeaveSchoolOrderRepository, TblOdrLeaveSchoolOrder>();
            }
            else
            {
                tblOdrRefundOrderRepository = new TblOdrRefundOrderRepository();
                tblOdrLeaveSchoolOrderRepository = new TblOdrLeaveSchoolOrderRepository();
            }

            var entity = new TblOdrLeaveSchoolOrder();
            var refundOrderInfo = tblOdrRefundOrderRepository.GetRefundOrderBySchOrStu(this._schoolId, this._studentId);
            if (refundOrderInfo != null)
            {
                entity = tblOdrLeaveSchoolOrderRepository.GetLeaveSchoolIdByOrder(refundOrderInfo.RefundOrderId);
            }
            return entity;
        }
        #endregion

        /// <summary>
        /// 暂不实现
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="cancelUserId">操作人Id</param>
        /// <param name="cancelUserName">操作人姓名</param>
        /// <param name="cancelRemark">备注</param>
        /// <returns></returns>
        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            throw new NotImplementedException();
        }
    }
}
