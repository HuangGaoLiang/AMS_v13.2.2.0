using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AMS.Storage.Repository.Orders;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 描述：退费订单－退班
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveClassOrderService : BaseRefundOrderService
    {
        protected override string OrderNoTag => "TB";                                           //订单号前缀
        private readonly Lazy<TblOdrLeaveClassOrderRepository> _tblOdrLeaveClassOrder;          //退费订单-退班
        private readonly Lazy<ViewLeaveClassDetailRepository> _viewLeaveClassDetailRepository;  //退班订单费用明细实体



        /// <summary>
        /// 描述：实例化要在指定校区的退班对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        public LeaveClassOrderService(string schoolId) : this(schoolId, 0)
        {
        }

        /// <summary>
        /// 描述：实例化一个学生要在指定校区的退班对象
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="schoolId">校区ID</param>
        /// <param name="studentId">学生ID</param>
        public LeaveClassOrderService(string schoolId, long studentId) : base(schoolId, studentId)
        {
            _tblOdrLeaveClassOrder = new Lazy<TblOdrLeaveClassOrderRepository>();
            _viewLeaveClassDetailRepository = new Lazy<ViewLeaveClassDetailRepository>();
        }

        /// <summary>
        /// 描述：获取订单详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单表主键</param>
        /// <param name="companayId">公司编号</param>
        /// <returns>订单详情信息</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1,异常描述：系统不存在该退费订单
        /// 异常ID：2,异常描述：系统不存在该退班信息
        /// 异常ID：3,异常描述：系统不存在该退费订单的支付信息
        /// </exception>
        public override IOrderDetailResponse GetOrderDetail(long refundOrderId, string companayId)
        {
            var result = new LeaveClassOrderDetailResponse();
            //获取退费订单信息
            var refundOrderEntity = base._tblOdrRefundOrderRepository.Value.GetRefundOrderIdByRefundOrder(refundOrderId);
            if (refundOrderEntity == null)
            {
                throw new BussinessException((byte)ModelType.Order, 1);
            }
            //获取退班信息
            var leaveClassOrder = _tblOdrLeaveClassOrder.Value.GetRefundOrderByLeaveClassOrder(refundOrderId);
            if (leaveClassOrder == null)
            {
                throw new BussinessException((byte)ModelType.Order, 2);
            }
            //获取退费订单的支付信息
            var refundPay = _tblOdrRefundPay.Value.GetRefundOrderIdByRefundPay(refundOrderId);
            if (refundPay == null)
            {
                throw new BussinessException((byte)ModelType.Order, 3);
            }
            //获取其他费用(扣款合计)
            var refundAmountTypeList = CostService.GetCosts(Dto.TypeCode.LEAVE_CLASS_FEE);
            var costList = _tblOdrRefundOrderCost.Value.GetrefundOrderIdByCost(refundOrderId).Select(x => new CostDetail
            {
                CostName = refundAmountTypeList.FirstOrDefault(k => k.CostId == x.CostId)?.CostName,
                Amount = x.Amount
            }).ToList();

            //获取退班的附件
            var attchmentService = new AttchmentService(base._schoolId);
            var attchmentList = attchmentService.GetAttchList(refundOrderId, AttchmentType.LEAVE_CLASS_APPLY);

            //获取退班的课程
            var leaveClassOrderDetailList = LeaveSchoolOrderService.GetRefundCourseAmountDetail(refundOrderId, companayId);

            result.OrderNo = refundOrderEntity.OrderNo;
            result.RefundDate = refundOrderEntity.CreateTime;
            result.CreatorId = refundOrderEntity.CreatorId;
            result.CreatorName = refundOrderEntity.CreatorName;
            result.EnrollCourseList = leaveClassOrderDetailList;
            result.StopClassDate = leaveClassOrder.StopClassDate;
            result.ReceiptStatus = leaveClassOrder.ReceiptStatus;
            result.TotalDeductAmount = costList;
            result.Amount = refundOrderEntity.Amount;
            result.RefundType = (RefundType)refundPay.RefundType;
            result.Reason = leaveClassOrder.Reason;
            result.AttachmentUrl = attchmentList;
            result.BankName = refundPay.BankName;
            result.BankCardNo = refundPay.BankCardNo;
            result.BankUserName = refundPay.BankUserName;

            return result;
        }


        /// <summary>
        /// 描述：办理前获取要办理的数据详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="iRequest">退班开始时间</param>
        /// <param name="companyId"></param>
        /// <returns>要退费的课次信息</returns>

        public override IRefundOrderTransactDetailReponse GetTransactDetail(IRefundOrderTransacDetailtRequest iRequest, string companyId)
        {
            //请求参数
            var request = iRequest as LeaveClassOrderTransactDetailRequest;
            //响应信息
            var respon = new LeaveClassOrderTransactDetailResponse
            {
                LeaveClassOrderDetailList = base.GetCourseLessonList(request.LeaveTime,companyId).Where(x => x.RefundAmount > 0).ToList()   //页面只显示退费金额大于0的数据
            };
            return respon;
        }

        /// <summary>
        /// 描述：办理
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="iRequest">要退费的信息</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：22,异常描述：找不到可退班的订单课程
        /// 异常ID：20,异常描述：该订单课程已退班退费
        /// </exception>
        public override void Transact(IRefundOrderTransactRequest iRequest,string companyId)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, this._schoolId, this._studentId.ToString()))
            {
                var request = iRequest as LeaveClassOrderAddRequest;
                var vailteRequest = EnrollOrderService.GetEnrollOrderItemIdByEnroOrderList(request.RefundCourseLesson);
                if (!request.RefundCourseLesson.Any())
                {
                    throw new BussinessException((byte)ModelType.Order, 22);
                }
                if (vailteRequest.Any(x => x.Status != OrderItemStatus.Enroll))  //如果订单课程不是已报名，则抛出异常
                {
                    throw new BussinessException((byte)ModelType.Order, 20);
                }

                //获取要退班的课次
                var courseLessonList = base.GetCourseLessonList(request.LeaveTime, companyId).Where(x => request.RefundCourseLesson.Contains(x.EnrollOrderItemId)).ToList();
                if (!courseLessonList.Any()) //如果要退费的订单课程为空，则抛出异常
                {
                    throw new BussinessException((byte)ModelType.Order, 22);
                }

                using (var unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        //1.退费订单
                        var tblOdrRefundOrder = AddOdrRefundOrder(request, courseLessonList, unitOfWork);
                        //2.退费订单课程明细
                        AddOdrRefundOrdeEnroll(courseLessonList, tblOdrRefundOrder, unitOfWork);
                        //3.其他费用
                        AddOdrRefundOrderCost(request, tblOdrRefundOrder, unitOfWork);
                        //4.支付信息
                        AddOdrRefundPay(request, tblOdrRefundOrder, unitOfWork);
                        //5.退班详细信息
                        var tblOdrLeaveClassOrder = AddOdrLeaveClassOrder(request, tblOdrRefundOrder, unitOfWork);
                        //6.添加附件
                        AddAttachment(request, tblOdrRefundOrder, unitOfWork);

                        //7、课次调用   
                        LeaveClassLessonFinisher leaveSchoolFinisher = new LeaveClassLessonFinisher(tblOdrRefundOrder, tblOdrLeaveClassOrder, request.RefundCourseLesson, unitOfWork);
                        LessonService lessonService = new LessonService(unitOfWork);
                        lessonService.Finish(leaveSchoolFinisher);//课次销毁


                        //8、资金调用
                        LeaveClassOrderTrade orderTrade = new LeaveClassOrderTrade(tblOdrRefundOrder, request.RefundType);
                        TradeService trade = new TradeService(orderTrade, unitOfWork);
                        trade.Trade();

                        //9、修改为退班状态
                        EnrollOrderService service = new EnrollOrderService(this._schoolId);
                        var enrollOrderItemIdList = courseLessonList.Select(x => x.EnrollOrderItemId).ToList();
                        service.ChangeToLeaveClass(enrollOrderItemIdList, unitOfWork).Wait();

                        //10.修改学生状态
                        var studentService = new StudentService(this._schoolId);
                        studentService.UpdateStudentStatusById(this._studentId, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }

                }
            }
        }

        /// <summary>
        /// 描述：暂不实现
        /// <para>作    者：</para>
        /// <para>创建时间：</para>
        /// </summary>
        /// <param name="searcher"></param>
        /// <returns></returns>
        public override LeaveSchoolOrderListResponse GetOrderList(IOrderListSearchRequest searcher)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 描述：写入退费订单主表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="request">退费班级信息</param>
        /// <param name="courseLessonList">退费的课程</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>退费订单信息</returns>

        private TblOdrRefundOrder AddOdrRefundOrder(LeaveClassOrderAddRequest request, List<RefundOrderTransactDetailListResponse> courseLessonList, UnitOfWork unitOfWork)
        {
            var refundOrderId = IdGenerator.NextId();
            var tblOdrRefundOrder = new TblOdrRefundOrder()   //退费订单
            {
                RefundOrderId = refundOrderId,
                SchoolId = base._schoolId,
                StudentId = base._studentId,
                OrderNo = base.CreateOrderNo(refundOrderId),
                OrderType = (int)OrderTradeType.LeaveClassOrder,
                TotalDeductAmount = courseLessonList.Sum(x => x.DeductAmount) + request.CostList.Sum(x => x.CostAmount),  //扣款合计=已上课次费用+扣款合计
                Amount = courseLessonList.Sum(x => x.RefundAmount) - request.CostList.Sum(x => x.CostAmount),             //实退金额=退费金额-扣款合计
                OrderStatus = request.RefundType == RefundType.RefundToParent ? (int)OrderStatus.WaitPay : (int)OrderStatus.Paid,
                CancelUserId = string.Empty,
                CancelDate = null,
                CancelRemark = string.Empty,
                CreateTime = DateTime.Now,
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName
            };
            var tblOdrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();
            tblOdrRefundOrderRepository.Add(tblOdrRefundOrder);
            return tblOdrRefundOrder;
        }
        /// <summary>
        /// 描述：写入退费订单课程明细表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="courseLessonList">退费的课程</param>
        /// <param name="tblOdrRefundOrder">退费订单信息</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>无</returns>

        private void AddOdrRefundOrdeEnroll(List<RefundOrderTransactDetailListResponse> courseLessonList, TblOdrRefundOrder tblOdrRefundOrder, UnitOfWork unitOfWork)
        {
            var refundOrdeEnrollList = new List<TblOdrRefundOrdeEnroll>();
            foreach (var item in courseLessonList)
            {
                var itemModel = new TblOdrRefundOrdeEnroll     //退费订单课程明细
                {
                    RefundOrderEnrollId = IdGenerator.NextId(),
                    SchoolId = base._schoolId,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    RefundOrderId = tblOdrRefundOrder.RefundOrderId,
                    LessonCount = item.LeaveSchoolLessons,
                    UseLessonCount = item.HaveClassLesson,
                    LessonPrice = item.TuitionFee + item.MaterialFee,
                    Amount = item.RefundAmount,
                    CreateTime = DateTime.Now
                };
                refundOrdeEnrollList.Add(itemModel);
            }
            var repository = unitOfWork.GetCustomRepository<TblOdrRefundOrdeEnrollRepository, TblOdrRefundOrdeEnroll>();
            repository.Add(refundOrdeEnrollList);
        }
        /// <summary>
        /// 描述：写入其他费用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="request">要添加的退班信息</param>
        /// <param name="tblOdrRefundOrder">退费订单主表信息</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>无</returns>

        private void AddOdrRefundOrderCost(LeaveClassOrderAddRequest request, TblOdrRefundOrder tblOdrRefundOrder, UnitOfWork unitOfWork)
        {
            var refundOrderCostList = new List<TblOdrRefundOrderCost>();
            foreach (var item in request.CostList)
            {
                var odrRefundOrderCost = new TblOdrRefundOrderCost   //退费订单-其他费用
                {
                    RefundClassCostId = IdGenerator.NextId(),
                    RefundOrderId = tblOdrRefundOrder.RefundOrderId,
                    SchoolId = base._schoolId,
                    CostId = item.CostId,
                    Amount = item.CostAmount,
                    CreateTime = DateTime.Now
                };
                refundOrderCostList.Add(odrRefundOrderCost);
            }
            var tblOdrRefundOrderCost = unitOfWork.GetCustomRepository<TblOdrRefundOrderCostRepository, TblOdrRefundOrderCost>();
            tblOdrRefundOrderCost.Add(refundOrderCostList);
        }
        /// <summary>
        /// 描述：写入支付信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="request">要添加的退班信息</param>
        /// <param name="tblOdrRefundOrder">退费订单主表信息</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>无</returns>

        private void AddOdrRefundPay(LeaveClassOrderAddRequest request, TblOdrRefundOrder tblOdrRefundOrder, UnitOfWork unitOfWork)
        {
            var odrRefundPay = new TblOdrRefundPay       //退费订单 - 支付信息
            {
                RefundPayId = IdGenerator.NextId(),
                RefundOrderId = tblOdrRefundOrder.RefundOrderId,
                SchoolId = base._schoolId,
                RefundType = (int)request.RefundType,
                Remark = request.Reason,
                BankName = request.BankName,
                BankCardNo = request.BankCardNo,
                BankUserName = request.BankUserName,
                CreateTime = DateTime.Now
            };
            var tblOdrRefundPay = unitOfWork.GetCustomRepository<TblOdrRefundPayRepository, TblOdrRefundPay>();
            tblOdrRefundPay.Add(odrRefundPay);
        }
        /// <summary>
        /// 描述：写入退班详细信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="request">要添加的退班信息</param>
        /// <param name="tblOdrRefundOrder">退费订单主表信息</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>退费订单信息</returns>
        private TblOdrLeaveClassOrder AddOdrLeaveClassOrder(LeaveClassOrderAddRequest request, TblOdrRefundOrder tblOdrRefundOrder, UnitOfWork unitOfWork)
        {
            var tblOdrLeaveClassOrder = new TblOdrLeaveClassOrder
            {
                RefundOrderId = tblOdrRefundOrder.RefundOrderId,
                SchoolId = base._schoolId,
                StopClassDate = request.LeaveTime,
                Reason = request.Reason,
                ReceiptStatus = (int)request.ReceiptType
            };
            var tblOdrLeaveClassOrderRepository = unitOfWork.GetCustomRepository<TblOdrLeaveClassOrderRepository, TblOdrLeaveClassOrder>();
            tblOdrLeaveClassOrderRepository.Add(tblOdrLeaveClassOrder);
            return tblOdrLeaveClassOrder;
        }
        /// <summary>
        /// 描述：添加附件
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="request">要添加的退费信息</param>
        /// <param name="tblOdrRefundOrder">退费订单主表信息</param>
        /// <param name="unitOfWork">事务工作单元</param>
        /// <returns>无</returns>

        private void AddAttachment(LeaveClassOrderAddRequest request, TblOdrRefundOrder tblOdrRefundOrder, UnitOfWork unitOfWork)
        {
            //添加上传申请表附件到数据库
            foreach (var item in request.AttachmentUrlList)
            {
                item.AttchmentType = AttchmentType.LEAVE_CLASS_APPLY;
                item.BusinessId = tblOdrRefundOrder.RefundOrderId;
            }

            var attchmentService = new AttchmentService(base._schoolId, unitOfWork);
            attchmentService.Add(request.AttachmentUrlList);

        }


        /// <summary>
        /// 描述：获取退班列表
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="searcher">列表筛选条件</param>
        /// <returns>退班课程明细列表分页</returns>

        public PageResult<LeaveClassOrderListResponse> GetLeaveClassOrderList(LeaveClassOrderListSearchRequest searcher)
        {
            var result = new PageResult<LeaveClassOrderListResponse>();

            var query = _viewLeaveClassDetailRepository.Value.GetLeaveClassByRefundOrder(searcher);
            result.Data = Mapper.Map<List<LeaveClassOrderListResponse>>(query.Data);
            result.CurrentPage = query.CurrentPage;
            result.PageSize = query.PageSize;
            result.TotalData = query.TotalData;

            return result;
        }



        /// <summary>
        /// 描述：获取要打印的收据详情
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2018-11-8</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>退费订单收据详情</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：1, 异常描述:系统不存在该退费订单
        /// 异常ID：2, 异常描述:系统不存在该退班信息
        /// </exception>
        public LeaveClassOrderReceiptDetailResponse GetPrintReceiptDetail(long refundOrderId, string companyId)
        {
            var result = new LeaveClassOrderReceiptDetailResponse();
            //获取退费订单信息
            var refundOrderEntity = _tblOdrRefundOrderRepository.Value.GetRefundOrderIdByRefundOrder(refundOrderId);
            if (refundOrderEntity == null)
            {
                throw new BussinessException((byte)ModelType.Order, 1);
            }
            //获取退班信息
            var leaveClassOrder = _tblOdrLeaveClassOrder.Value.GetRefundOrderByLeaveClassOrder(refundOrderId);
            if (leaveClassOrder == null)
            {
                throw new BussinessException((byte)ModelType.Order, 2);
            }
            //获取其他费用(扣款合计)
            var refundAmountTypeList = CostService.GetCosts(Dto.TypeCode.LEAVE_CLASS_FEE);
            var costList = _tblOdrRefundOrderCost.Value.GetrefundOrderIdByCost(refundOrderId).Select(x => new CostDetail
            {
                CostName = refundAmountTypeList.FirstOrDefault(k => k.CostId == x.CostId)?.CostName,
                Amount = x.Amount
            }).ToList();

            //获取退班的课程
            var leaveClassOrderDetailList = LeaveSchoolOrderService.GetRefundCourseAmountDetail(refundOrderId, companyId);

            var printNo = PrintCounterService.Print(base._schoolId, PrintBillType.LeaveClass);

            result.RefundDate = refundOrderEntity.CreateTime;
            result.ReceiptStatus = leaveClassOrder.ReceiptStatus;
            result.StopClassDate = leaveClassOrder.StopClassDate;
            result.TotalDeductAmount = costList;
            result.RefundAmount = refundOrderEntity.Amount; //实退金额=已上课次扣费金额+扣款合计
            result.PrintNo = printNo;
            result.RefundCourseList = leaveClassOrderDetailList;

            OrgService orgService = new OrgService();
            var schoolList = orgService.GetAllSchoolList().FirstOrDefault(x => x.SchoolId.Trim() == leaveClassOrder.SchoolId.Trim());
            result.RefundUnit = schoolList == null ? "" : schoolList.SchoolName;
            return result;
        }
        /// <summary>
        /// 此场景暂时没有，此方法暂不实现
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="cancelUserId">操作人ID</param>
        /// <param name="cancelUserName">操作人姓名</param>
        /// <param name="cancelRemark">备注</param>
        /// <returns></returns>
        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            throw new NotImplementedException();
        }
    }
}
