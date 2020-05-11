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
using Jerrisoft.Platform.Log;
using Jerrisoft.Platform.Public.PageExtensions;

namespace AMS.Service
{
    /// <summary>
    /// 退费订单－转校
    /// <para>作    者：zhiwei.Tnag</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class ChangeSchoolOrderService : BaseRefundOrderService
    {
        private readonly Lazy<ViewChangeSchooolOrderRepository> _viewChangenSchoolOrderRepository = new Lazy<ViewChangeSchooolOrderRepository>();

        protected override string OrderNoTag => "Z";



        /// <summary>
        /// 根据校区学生实例化一个转校对象
        /// <para>作    者：</para>
        /// <para>创建时间：</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public ChangeSchoolOrderService(string schoolId) : this(schoolId, 0)
        {

        }


        /// <summary>
        /// 根据校区学生实例化一个转校对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public ChangeSchoolOrderService(string schoolId, long studentId) : base(schoolId, studentId)
        {

        }

        #region 获取订单详情
        /// <summary>
        /// 获取订单详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="orderId">转校订单Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>转校订单详情</returns>
        /// <exception cref="BussinessException">
        /// 异常ID:1,转校订单信息为空
        /// </exception>
        public override IOrderDetailResponse GetOrderDetail(long orderId, string companyId)
        {
            //1.获取转校订单信息
            ViewChangeSchooolOrder cso = new ViewChangeSchooolOrderRepository().GetChangeSchooolOrder(orderId);
            if (cso == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //2.获取转校订单详情
            ChangeSchoolOrderDetailResponse changeSchoolOrderDetail = this.GetChangeSchoolOrderDetailResponse(cso);

            return changeSchoolOrderDetail;
        }

        /// <summary>
        /// 获取转校扣款明细项
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="orderId">转校订单Id</param>
        /// <returns>扣费明细集合</returns>
        private List<ChangeSchoolCostResponse> GetChangeSchoolCostResponseList(long orderId)
        {
            List<ChangeSchoolCostResponse> res = new List<ChangeSchoolCostResponse>();

            //获取其他费用集合
            List<TblOdrRefundOrderCost> refundOrderCost = base._tblOdrRefundOrderCost
                .Value
                .GetrefundOrderIdByCost(orderId);
            if (refundOrderCost == null || !refundOrderCost.Any())
            {
                return res;
            }

            //获取指定费用类别的费用
            List<CostResponse> costs = CostService.GetCosts(Dto.TypeCode.CHANGE_SCHOOL_FEE);

            res = (from a in costs
                   join b in refundOrderCost on a.CostId equals b.CostId
                   select new ChangeSchoolCostResponse
                   {
                       CostAmount = b.Amount,
                       CostId = a.CostId,
                       Name = a.CostName
                   }).ToList();

            return res;
        }

        /// <summary>
        /// 转校操作日志
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="orderId">转校订单Id</param>
        /// <returns>操作日志集合</returns>
        private List<OperationLogResponse> GetOperationLogResponseList(long orderId)
        {
            OperationLogService service = new OperationLogService();
            List<TblDatOperationLog> operationLogs = service.GetList(orderId);

            //操作日志
            List<OperationLogResponse> res = operationLogs.Select(x => new OperationLogResponse
            {
                OperationTime = x.CreateTime,
                OperatorName = x.OperatorName,
                OperationFlowStatus = (OperationFlowStatus)x.FlowStatus,
                ProcessStatusName = EnumName.GetDescription(typeof(OperationFlowStatus), x.FlowStatus),
                SchoolName = OrgService.GetSchoolBySchoolId(x.SchoolId)?.SchoolName,
                Remark = x.Remark
            }).ToList();

            return res;
        }

        /// <summary>
        /// 获取申请表附件地址
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="orderId">转校订单Id</param>
        /// <returns>申请表附件地址集合</returns>
        private List<string> GetAttchmentList(long orderId)
        {
            AttchmentService attchmentService = new AttchmentService(base._schoolId);

            List<string> attchments = attchmentService.GetAttchList(orderId, AttchmentType.ORDER_TRANSFERSCHOOL)
                 .Select(x => x.Url)
                 .ToList();

            return attchments;
        }

        /// <summary>
        /// 获取转校账户信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="csoId">转校订单Id</param>
        /// <returns>账户信息</returns>
        private TransferAccountsResponse GetTransferAccountsResponse(long orderId)
        {
            TblOdrRefundPay pay = base._tblOdrRefundPay.Value.GetRefundOrderIdByRefundPay(orderId);

            return new TransferAccountsResponse
            {
                AccountName = pay?.BankUserName ?? string.Empty,
                BankCardNumber = pay?.BankCardNo ?? string.Empty,
                IssuingBank = pay?.BankName ?? string.Empty
            };
        }

        /// <summary>
        /// 获取转校订单详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="cso">转校订单</param>
        /// <returns>转校订单详情</returns>
        private ChangeSchoolOrderDetailResponse GetChangeSchoolOrderDetailResponse(ViewChangeSchooolOrder cso)
        {
            ChangeSchoolOrderDetailResponse res = new ChangeSchoolOrderDetailResponse
            {
                CostTotal = cso.TotalDeductAmount,
                OrderNo = cso.OrderNo,
                Amount = cso.Amount + cso.TransFromBalance,//实际金额=退费课程金额+学生余额
                ReceiptStatus = cso.ReceiptStatus,
                OutWhy = cso.Remark,
                OutTime = cso.OutDate,
                TransFromBalance = cso.TransFromBalance,
                TotalHaveClassLesson = cso.TotalUseLessonCount,
                OperationTime = cso.CreateTime,
                OperatorName = cso.CreatorName,
                OrderStatusName = EnumName.GetDescription(typeof(OrderStatusTransferResponse), cso.OrderStatus),
                OrderStatus = cso.OrderStatus,
                InSchoolName = OrgService.GetSchoolBySchoolId(cso.InSchoolId)?.SchoolName,
                ToSchoolName = OrgService.GetSchoolBySchoolId(cso.OutSchoolId)?.SchoolName,
                Cost = this.GetChangeSchoolCostResponseList(cso.RefundOrderId),
                FileUrls = this.GetAttchmentList(cso.RefundOrderId),
                OperationLog = this.GetOperationLogResponseList(cso.RefundOrderId),
                TransferAccounts = this.GetTransferAccountsResponse(cso.RefundOrderId),
                TransferCourseFeeDetail = this.GetTransferCourseFeeDetailResponseList(cso.RefundOrderId),
                TotalLessonFee = 0
            };

            //统计应扣除上课费用合计
            res.TotalLessonFee = res.TransferCourseFeeDetail.Sum(x => x.DeductAmount);

            return res;
        }

        /// <summary>
        /// 获取转校课程费用明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="orderId">转校订单Id</param>
        /// <returns>转校课程费用明细集合</returns>
        private List<TransferCourseFeeDetailResponse> GetTransferCourseFeeDetailResponseList(long orderId)
        {
            //定义返回值
            List<TransferCourseFeeDetailResponse> res = new List<TransferCourseFeeDetailResponse>();

            //1.获取退费订单课程明细信息
            List<TblOdrRefundOrdeEnroll> refundOrdeEnrolls = base._tblOdrRefundOrdeEnrollRepository
              .Value
              .GetRefundOrderByOrderEnroll(orderId);

            if (refundOrdeEnrolls.Count == 0)
            {
                return res;
            }

            //2.获取所有的课程信息
            List<TblDatCourse> courseList = CourseService.GetAllAsync().Result;

            //3.获取所有的学期类型
            List<TermTypeResponse> termTypeList = new TermTypeService().GetAll();

            //4.获取所有的课程级别集合
            List<CourseLevelResponse> courseLevelList = CourseLevelService.GetCourseLevelList().Result;

            //5.获取报名订单课程信息                                                                            
            List<EnrollOrderResponse> enrollOrderItemList = EnrollOrderService
                .GetEnrollOrderItemIdByEnroOrderList(refundOrdeEnrolls.Select(x => x.EnrollOrderItemId));

            //6.组合数据
            foreach (var item in refundOrdeEnrolls)
            {
                var orderItem = enrollOrderItemList.FirstOrDefault(x => x.EnrollOrderItemId == item.EnrollOrderItemId);

                if (orderItem == null)
                {
                    continue;
                }

                var model = new TransferCourseFeeDetailResponse
                {
                    Year = orderItem.Year,
                    TermTypeName = termTypeList.FirstOrDefault(k => k.TermTypeId == orderItem.TermTypeId)?.TermTypeName ?? string.Empty,
                    EnrollOrderItemId = orderItem.EnrollOrderItemId,
                    CourseName = courseList.FirstOrDefault(k => k.CourseId == orderItem.CourseId)?.ShortName ?? string.Empty,
                    CourseLevelName = courseLevelList.FirstOrDefault(k => k.CourseLevelId == orderItem.CourseLevelId)?.LevelCnName ?? string.Empty,
                    ClassTimes = orderItem.ClassTimes,
                    PayAmount = orderItem.PayAmount,
                    HaveClassLesson = item.UseLessonCount,
                    LessonFee = item.LessonPrice,
                    DeductAmount = item.LessonPrice * item.UseLessonCount
                };

                res.Add(model);
            }

            return res;
        }
        #endregion

        /// <summary>
        /// 
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
        /// 本校转出-费用明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="iRequest">退费订单办理前获取详情的参数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>退费订单办理前的详情信息</returns>
        /// <exception cref="BussinessException">
        /// 异常ID:1,退费订单办理前获取详情的参数为空
        /// </exception>
        public override IRefundOrderTransactDetailReponse GetTransactDetail(IRefundOrderTransacDetailtRequest iRequest, string companyId)
        {
            ChangeSchoolOrderTransactDetailRequest request = iRequest as ChangeSchoolOrderTransactDetailRequest;

            if (request == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            var result = new ChangeSchoolOrderTransactDetailResponse
            {
                TransferCourseFeeDetail = this.GetTransferCourseFeeDetail(request.ChangeSchoolTime)
            };

            return result;
        }

        /// <summary>
        /// 转校-课程费用明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="changeSchoolTime">转校时间</param>
        /// <returns>转校课程费用明细集合</returns>
        private List<TransferCourseFeeDetailResponse> GetTransferCourseFeeDetail(DateTime changeSchoolTime)
        {
            List<TransferCourseFeeDetailResponse> result = new List<TransferCourseFeeDetailResponse>();

            EnrollOrderService enrollOrderService = new EnrollOrderService(base._schoolId);
            var orderItems = enrollOrderService.GetStudentEnroOrderItem(base._studentId, changeSchoolTime);

            if (orderItems.Count == 0)
            {
                return result;
            }

            //获取所有的课程信息
            var courseList = CourseService.GetAllAsync().Result;

            //获取所有的学期类型
            var termTypeList = new TermTypeService().GetAll();

            //获取所有的课程级别集合
            var courseLevelList = CourseLevelService.GetCourseLevelList().Result;

            StudentTimetableService studentTimetableService = new StudentTimetableService(base._schoolId, base._studentId);
            //可转校课次
            var scheduledLessonList = studentTimetableService.GetLeaveSchoolLessonCount(changeSchoolTime);

            foreach (var item in orderItems)
            {
                var scheduledLesson = scheduledLessonList.FirstOrDefault(m => m.EnrollOrderItemId == item.EnrollOrderItemId);
                //可转课次
                var turnableLesson = scheduledLesson?.Count ?? 0;

                if (item.ClassTimes == item.ClassTimesUse && turnableLesson == 0)
                {
                    continue;
                }

                //学期类型名称
                string termTypeName = termTypeList.FirstOrDefault(k => k.TermTypeId == item.TermTypeId)?.TermTypeName ?? string.Empty;

                //课程简称
                string courseName = courseList.FirstOrDefault(k => k.CourseId == item.CourseId)?.ShortName ?? string.Empty;

                //等级中文名称
                string courseLvName = courseLevelList.FirstOrDefault(k => k.CourseLevelId == item.CourseLevelId)?.LevelCnName ?? string.Empty;

                //实收金额=(学费单价(实价)+杂费单价(实价))*报名课次
                decimal payAmount = (item.TuitionFeeReal + item.MaterialFeeReal) * item.ClassTimes;

                //实际单次课时费=实收金额 / 报名课次
                decimal lessonFee = Math.Round(payAmount / item.ClassTimes, 2); //保留两位小数

                decimal deductAmount = 0.00M;
                int haveClassLesson = 0;

                if (item.ClassTimes != 0)//已排课未排完
                {
                    ////已上课次=报名课次-(未排课课次=报名课次-已排课课次)-可转校课次
                    //haveClassLesson = item.ClassTimes - (item.ClassTimes - item.ClassTimesUse) - turnableLesson;
                    //已上课次=已排课次-可转校课次
                    haveClassLesson = item.ClassTimesUse - turnableLesson;

                    //应扣除上课费用 = 已上课课次 * 实际单次课时费
                    deductAmount = Math.Round(haveClassLesson * lessonFee, 2);
                }

                TransferCourseFeeDetailResponse transferCourseFeeDetailResponse = new TransferCourseFeeDetailResponse
                {
                    Year = item.Year,
                    TermTypeName = termTypeName,
                    EnrollOrderItemId = item.EnrollOrderItemId,
                    CourseName = courseName,
                    CourseLevelName = courseLvName,
                    ClassTimes = item.ClassTimes,
                    PayAmount = payAmount,
                    HaveClassLesson = haveClassLesson,
                    DeductAmount = deductAmount,
                    LessonFee = lessonFee
                };

                result.Add(transferCourseFeeDetailResponse);
            }

            return result;
        }

        #region 申请转校

        /// <summary>
        /// 办理(提交给财务)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="iRequest">退费订单办理数据</param>
        /// <param name="companyId">公司编号</param>
        /// <exception cref="BussinessException">
        /// 异常ID:18,没有可以转校的课程费用明细
        /// </exception>
        public override void Transact(IRefundOrderTransactRequest iRequest, string companyId)
        {
            var request = iRequest as ChangeSchoolOrderAddRequest;

            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, _studentId.ToString()))
            {
                //1、校验
                this.TransactVerify(request);

                //获取转校课程费用明细
                var transferCourseFeeDetail = this.ConfirmTransferCourseFeeDetail(request.TransferInformation.TransferDate, request.FeeDetail.EnrollOrderItemId);
                if (transferCourseFeeDetail.Count == 0)
                {
                    //没有可转校的课程
                    throw new BussinessException(ModelType.Order, 18);
                }

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.BeginTransaction();
                    try
                    {
                        //2、添加退费订单
                        TblOdrRefundOrder refundOrder = this.AddRefundOrder(request, transferCourseFeeDetail, unitOfWork);

                        //3、添加退费订单-转校
                        this.AddRefundChangeSchoolOrder(request, refundOrder.RefundOrderId, transferCourseFeeDetail, unitOfWork);

                        //4、添加退费订单-订单课程明细
                        this.AddRefundOrdeEnroll(request.FeeDetail.EnrollOrderItemId, refundOrder.RefundOrderId, transferCourseFeeDetail, unitOfWork);

                        //5、添加退费订单-其他费用
                        this.AddRefundOrderCost(request.FeeDetail.Cost, refundOrder.RefundOrderId, unitOfWork);

                        //6、添加退费订单-支付信息
                        this.AddRefundPay(request.TransferAccounts, refundOrder.RefundOrderId, unitOfWork);

                        //7、上传申请附件表
                        this.AddAttachment(request.TransferInformation.Attachments, refundOrder.RefundOrderId, unitOfWork);

                        //8、冻结余额
                        this.FreezeBalance(refundOrder, unitOfWork);

                        //9、销毁课次
                        this.DestroyLesson(refundOrder.RefundOrderId, unitOfWork);

                        //10、操作日记
                        this.WriteOperationLog(refundOrder.RefundOrderId, OperationFlowStatus.Submit, request.OperatorId, request.OperatorName, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }
                }
            }

            //11、更新学生课次
            this.UpdateStudentlessons();
        }

        /// <summary>
        /// 获取转校的报名课程订单Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <returns>转校的报名课程订单Id集合</returns>
        private List<long> GetStudentTransferOrder()
        {
            //学生的正常转校订单
            var stuOrders = _viewChangenSchoolOrderRepository.Value.GetStudentChangeSchoolOrder(_studentId, _schoolId);

            if (stuOrders.Count == 0)
            {
                return new List<long>();
            }

            var enrollOrderItemIds = stuOrders.Select(m => m.RefundOrderId).ToList();

            var refundOrdeEnrolls = base._tblOdrRefundOrdeEnrollRepository.Value.GetRefundOrderByOrderEnroll(enrollOrderItemIds);

            return refundOrdeEnrolls.Select(m => m.EnrollOrderItemId).ToList();
        }

        /// <summary>
        /// 更新学生课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        private void UpdateStudentlessons()
        {
            var stuService = new StudentService(this._schoolId);
            Task.Run(() =>
            {
                try
                {
                    stuService.UpdateStudentStatusById(this._studentId);
                }
                catch (Exception ex)
                {
                    LogWriter.Write(this, $"申请转校更新学生课次异常:{ex.Message}", LoggerType.Debug);
                }
            });
        }

        /// <summary>
        /// 确定要转出的课程明细
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="transferDate">转出日期</param>
        /// <param name="enrollOrderItemId">报名订单课程明细Id</param>
        /// <returns>确定要转出的课程明细集合</returns>
        private List<TransferCourseFeeDetailResponse> ConfirmTransferCourseFeeDetail(DateTime transferDate, List<long> enrollOrderItemId)
        {
            return this.GetTransferCourseFeeDetail(transferDate)
                    .Where(x => enrollOrderItemId.Contains(x.EnrollOrderItemId))
                    .ToList();
        }

        /// <summary>
        /// 转校校验
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">转校订单添加数据</param>
        /// <exception cref="BussinessException">
        /// 异常ID：2,异常描述：传入参数为空
        /// </exception>
        private void TransactVerify(ChangeSchoolOrderAddRequest request)
        {
            //1、参数校验是否非空
            if (request == null)
            {
                throw new BussinessException(ModelType.Default, 2);
            }

            //2.校验转入校区信息
            this.TransactVerifySchool(request.TransferInformation.TransferToSchoolId);

            //3.校验转校课程
            this.TransactVerifyCourse(request.FeeDetail.EnrollOrderItemId);

            //4.转出余额不能大于账户余额
            if (!WalletService.IsWalletSufficient(_schoolId, _studentId, request.FeeDetail.PayBalance))
            {
                throw new BussinessException(ModelType.Order, 15);
            }

            //5.必须上传一张申请表
            if (request.TransferInformation.Attachments.Count == 0)
            {
                throw new BussinessException(ModelType.Order, 19);
            }
        }

        /// <summary>
        /// 校验转校课程
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="enrollOrderItemIds">报名订单课程Id</param>
        /// <exception cref="BussinessException">
        /// 异常ID：11,异常描述：必须转出至少一个课程
        /// 异常ID：18,异常描述：没有可转校的课程
        /// </exception>
        private void TransactVerifyCourse(List<long> enrollOrderItemIds)
        {
            //1.必须转出至少一个课程
            if (enrollOrderItemIds.Count == 0)
            {
                throw new BussinessException(ModelType.Order, 11);
            }

            //2.校验转出课程是否正常
            List<long> stuOrderItem = this.GetStudentTransferOrder();    //获取已转校的报名课程订单Id
            var intersects = stuOrderItem.Intersect(enrollOrderItemIds); //取交集
            if (intersects.Any())
            {
                throw new BussinessException(ModelType.Order, 18);
            }
        }

        /// <summary>
        /// 转校校验对方校区
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="transferToSchoolId">转入校区Id</param>
        /// <exception cref="BussinessException">
        /// 异常ID：9,异常描述：同一校区不能互转
        /// 异常ID：17,异常描述：转入校区不存在
        /// </exception>
        private void TransactVerifySchool(string transferToSchoolId)
        {
            //1.同一校区不能互转
            if (transferToSchoolId == this._schoolId)
            {
                throw new BussinessException(ModelType.Order, 9);
            }

            //2.校验对方校区是否存在
            var schoolInfo = OrgService.GetSchoolBySchoolId(transferToSchoolId);
            if (schoolInfo == null)
            {
                throw new BussinessException(ModelType.Order, 17);
            }
        }

        /// <summary>
        /// 添加退费订单
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="request">转校订单添加数据</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <remarks>退费订单信息</remarks>
        private TblOdrRefundOrder AddRefundOrder(ChangeSchoolOrderAddRequest request, IReadOnlyList<TransferCourseFeeDetailResponse> transferCourseFeeDetail, UnitOfWork unitOfWork)
        {
            TblOdrRefundOrderRepository odrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();

            long refundOrderId = IdGenerator.NextId();
            TblOdrRefundOrder refundOrder = new TblOdrRefundOrder
            {
                RefundOrderId = refundOrderId,
                TotalDeductAmount = this.GetCostItemTotal(request.FeeDetail.Cost),//扣款项统计
                Amount = this.GetTransferAmount(request, transferCourseFeeDetail),
                OrderNo = base.CreateOrderNo(refundOrderId),
                CreateTime = DateTime.Now,
                CreatorId = request.OperatorId,
                CreatorName = request.OperatorName,
                OrderStatus = (int)OrderStatus.WaitPay,
                OrderType = (int)OrderTradeType.ChangeSchool,
                SchoolId = this._schoolId,
                StudentId = this._studentId,
                CancelDate = null,
                CancelRemark = string.Empty,
                CancelUserId = string.Empty,
            };

            odrRefundOrderRepository.Add(refundOrder);

            return refundOrder;
        }

        /// <summary>
        /// 添加退费订单-转校
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="request">转校订单添加数据</param>
        /// <param name="refundOrderId">退费订单Id</param>
        private void AddRefundChangeSchoolOrder(ChangeSchoolOrderAddRequest request, long refundOrderId, IReadOnlyList<TransferCourseFeeDetailResponse> transferCourseFeeDetail, UnitOfWork unitOfWork)
        {
            TblOdrRefundChangeSchoolOrderRepository repository =
                unitOfWork.GetCustomRepository<TblOdrRefundChangeSchoolOrderRepository, TblOdrRefundChangeSchoolOrder>();

            TblOdrRefundChangeSchoolOrder refundChangeSchoolOrder = new TblOdrRefundChangeSchoolOrder
            {
                SchoolId = this._schoolId,
                OutSchoolId = this._schoolId,//转出校区
                InSchoolId = request.TransferInformation.TransferToSchoolId,//转入校区
                OutDate = request.TransferInformation.TransferDate,
                ReceiptStatus = request.FeeDetail.ReceiptStatus,
                RefundOrderId = refundOrderId,
                Remark = request.TransferInformation.ReasonForTransfer,
                TransFromBalance = request.FeeDetail.PayBalance,
                TotalRefundLessonCount = this.GetTotalRefundLessonCount(request.FeeDetail.EnrollOrderItemId, transferCourseFeeDetail),
                TotalUseLessonCount = this.GetTotalUseLessonCount(request.FeeDetail.EnrollOrderItemId, transferCourseFeeDetail)
            };

            repository.Add(refundChangeSchoolOrder);
        }

        /// <summary>
        /// 添加退费订单-订单课程明细
        /// 更新报名课程明细为转校
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="enrollOrderItemIds">报名订单课程明细ID</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="transferCourseFeeDetail">转校课程费用明细</param>
        /// <param name="unitOfWork">工作单元</param>
        private void AddRefundOrdeEnroll(IEnumerable<long> enrollOrderItemIds, long refundOrderId, IReadOnlyList<TransferCourseFeeDetailResponse> transferCourseFeeDetail, UnitOfWork unitOfWork)
        {
            TblOdrRefundOrdeEnrollRepository odrRefundOrdeEnrollRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrdeEnrollRepository, TblOdrRefundOrdeEnroll>();

            EnrollOrderService enrollOrderService = new EnrollOrderService(base._schoolId);

            foreach (var id in enrollOrderItemIds)
            {
                var feeDetail = transferCourseFeeDetail.FirstOrDefault(x => x.EnrollOrderItemId == id);
                if (feeDetail == null)
                {
                    continue;
                }

                var model = new TblOdrRefundOrdeEnroll
                {
                    SchoolId = this._schoolId,
                    CreateTime = DateTime.Now,
                    EnrollOrderItemId = id,
                    RefundOrderEnrollId = IdGenerator.NextId(),
                    RefundOrderId = refundOrderId,
                    LessonCount = feeDetail.ClassTimes - feeDetail.HaveClassLesson,   //退课课次=报名课次-已上课课次
                    LessonPrice = feeDetail.LessonFee,
                    UseLessonCount = feeDetail.HaveClassLesson,
                    Amount = feeDetail.PayAmount - feeDetail.DeductAmount             //退费金额=实收金额-应扣除上课费用 
                };

                odrRefundOrdeEnrollRepository.Add(model);

                enrollOrderService.ChangeToChangeSchool(id, unitOfWork);
            }
        }

        /// <summary>
        /// 添加扣费项
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="costs">扣费项</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void AddRefundOrderCost(List<CostRequest> costs, long refundOrderId, UnitOfWork unitOfWork)
        {
            if (!costs.Any())
            {
                return;
            }

            var odrRefundOrderCost = unitOfWork.GetCustomRepository<TblOdrRefundOrderCostRepository, TblOdrRefundOrderCost>();

            List<TblOdrRefundOrderCost> refundOrderCosts = costs.Select(m => new TblOdrRefundOrderCost
            {
                Amount = m.CostAmount,
                SchoolId = base._schoolId,
                CostId = m.CostId,
                CreateTime = DateTime.Now,
                RefundClassCostId = IdGenerator.NextId(),
                RefundOrderId = refundOrderId
            }).ToList();

            odrRefundOrderCost.Add(refundOrderCosts);
        }

        /// <summary>
        /// 添加退费订单-支付信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="request">账户信息</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void AddRefundPay(TransferAccountsRequest request, long refundOrderId, UnitOfWork unitOfWork)
        {
            var repository = unitOfWork.GetCustomRepository<TblOdrRefundPayRepository, TblOdrRefundPay>();

            //用户未填写退费支付信息则添加默认值(占位)
            if (request == null)
            {
                request = new TransferAccountsRequest
                {
                    AccountName = string.Empty,
                    BankCardNumber = string.Empty,
                    IssuingBank = string.Empty
                };
            }

            TblOdrRefundPay refundPay = new TblOdrRefundPay
            {
                BankCardNo = request.BankCardNumber,
                BankName = request.IssuingBank,
                BankUserName = request.AccountName,
                CreateTime = DateTime.Now,
                RefundOrderId = refundOrderId,
                RefundPayId = IdGenerator.NextId(),
                RefundType = (int)RefundType.Default,
                Remark = string.Empty,
                SchoolId = this._schoolId
            };

            repository.Add(refundPay);
        }

        /// <summary>
        /// 上传转校申请表附件
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="attachments">一组附件信息</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void AddAttachment(List<AttchmentAddRequest> attachments, long refundOrderId, UnitOfWork unitOfWork)
        {
            attachments.ForEach(m =>
            {
                m.AttchmentType = AttchmentType.ORDER_TRANSFERSCHOOL;
                m.BusinessId = refundOrderId;
            });

            AttchmentService attchmentService = new AttchmentService(base._schoolId, unitOfWork);

            attchmentService.Add(attachments);
        }

        /// <summary>
        /// 销毁课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrderId">订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void DestroyLesson(long refundOrderId, UnitOfWork unitOfWork)
        {
            ChangeSchoolLessonFinisher lessonFinisher = new ChangeSchoolLessonFinisher(refundOrderId, unitOfWork);
            LessonService lessonService = new LessonService(unitOfWork);
            lessonService.Finish(lessonFinisher);//课次销毁
        }

        /// <summary>
        /// 冻结余额
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrder">订单</param>
        /// <param name="unitOfWork">工作单元</param>
        private void FreezeBalance(TblOdrRefundOrder refundOrder, UnitOfWork unitOfWork)
        {
            ChangeSchoolOrderTrade orderTrade = new ChangeSchoolOrderTrade(
                refundOrder,
                unitOfWork);

            FrozeTradeService trade = new FrozeTradeService(orderTrade, unitOfWork);
            trade.Trade();
        }

        /// <summary>
        /// 业务记录流程操作记录
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrderId">订单Id</param>
        /// <param name="flowStatus">操作状态</param>
        /// <param name="remark">备注</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="operatorName">操作人名称</param>
        private void WriteOperationLog(long refundOrderId, OperationFlowStatus flowStatus, string operatorId, string operatorName, UnitOfWork unitOfWork, string remark = "")
        {
            TblDatOperationLog log = new TblDatOperationLog
            {
                BusinessType = (int)OrderTradeType.ChangeSchool,
                OperationLogId = IdGenerator.NextId(),
                OperatorId = operatorId,
                OperatorName = operatorName,
                SchoolId = this._schoolId,
                Remark = remark,
                FlowStatus = (int)flowStatus,
                CreateTime = DateTime.Now,
                BusinessId = refundOrderId,
            };

            var operationLogService = new OperationLogService();
            operationLogService.Add(log, unitOfWork);
        }

        /// <summary>
        /// 获取已上课总课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程明细ID</param>
        /// <param name="transferCourseFeeDetail">转校课程费用明细</param>
        /// <returns>已上课总课次</returns>
        private int GetTotalUseLessonCount(IReadOnlyList<long> enrollOrderItemId, IReadOnlyList<TransferCourseFeeDetailResponse> transferCourseFeeDetail)
        {
            //已上课次合计
            int haveClassLessonTotal = transferCourseFeeDetail.Where(x => enrollOrderItemId.Contains(x.EnrollOrderItemId)).Sum(x => x.HaveClassLesson);

            return haveClassLessonTotal;
        }

        /// <summary>
        /// 退课总课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="enrollOrderItemId">报名课程明细ID</param>
        /// <param name="transferCourseFeeDetail">转校课程费用明细</param>
        /// <returns>退课总课次</returns>
        private int GetTotalRefundLessonCount(IReadOnlyList<long> enrollOrderItemId, IReadOnlyList<TransferCourseFeeDetailResponse> transferCourseFeeDetail)
        {
            //报名课次合计
            int classTimesTotal = transferCourseFeeDetail.Where(x => enrollOrderItemId.Contains(x.EnrollOrderItemId)).Sum(x => x.ClassTimes);

            //已上课次合计
            int haveClassLessonTotal = this.GetTotalUseLessonCount(enrollOrderItemId, transferCourseFeeDetail);

            //退课总课次
            int refundLessonTotal = classTimesTotal - haveClassLessonTotal;

            return refundLessonTotal;
        }

        /// <summary>
        /// 统计扣款项金额
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-12</para>
        /// </summary>
        /// <param name="costRequests">扣款信息集合</param>
        /// <returns>合计扣款项金额</returns>
        private decimal GetCostItemTotal(List<CostRequest> costRequests)
        {
            decimal costAmount = 0;//扣费金额总计

            if (costRequests != null && costRequests.Any())
            {
                costAmount = costRequests.Sum(x => x.CostAmount);
            }

            return costAmount;
        }

        /// <summary>
        /// 获取转出金额
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="request">转校订单添加数据</param>
        /// <param name="transferCourseFeeDetail">转校课程费用明细</param>
        /// <returns>转出金额</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：4 异常描述：系统不存在该学生转校课程
        /// </exception>
        private decimal GetTransferAmount(ChangeSchoolOrderAddRequest request, IReadOnlyCollection<TransferCourseFeeDetailResponse> transferCourseFeeDetail)
        {
            if (transferCourseFeeDetail == null && !transferCourseFeeDetail.Any())
            {
                //系统不存在该学生转校课程
                throw new BussinessException(ModelType.Order, 4);
            }

            //应扣除的上课费用合计
            decimal deductAmountTotal = transferCourseFeeDetail.Sum(x => x.DeductAmount);

            //实收金额合计
            decimal payAmountTotal = transferCourseFeeDetail.Sum(x => x.PayAmount);

            //扣费项合计
            decimal costTotal = this.GetCostItemTotal(request.FeeDetail.Cost);

            //转出金额=所选的课程的实收金额之和- 应扣除上课费用合计-扣款合计
            decimal transferAmount = payAmountTotal - deductAmountTotal - costTotal;

            return transferAmount;
        }
        #endregion

        #region 转出/转入列表

        /// <summary>
        /// 获取转出列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="searcher">转校列表搜索条件参数</param>
        /// <remarks>转出列表</remarks>
        public static PageResult<ChangeSchoolOrderListResponse> GetChangeOutList(string schoolId, ChangeSchoolOrderListSearchRequest searcher)
        {
            //1.获取转出列表信息
            PageResult<ViewChangeSchooolOrder> res = new ViewChangeSchooolOrderRepository()
                .GetChangeOutList(schoolId, searcher, GetSearchForStudents(searcher.Key));

            //2.组合转出列表数据
            return new PageResult<ChangeSchoolOrderListResponse>
            {
                CurrentPage = res.CurrentPage,
                PageSize = res.PageSize,
                TotalData = res.TotalData,
                Data = GetChangeSchoolOrderListResponseList(res.Data)
            };
        }

        /// <summary>
        /// 获取转入列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间： 2019-02-15</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="searcher">转校列表搜索条件参数</param>
        /// <remarks>转入列表</remarks>
        public static PageResult<ChangeSchoolOrderListResponse> GetChangeInList(string schoolId, ChangeSchoolOrderListSearchRequest searcher)
        {
            //1.获取转入列表信息
            PageResult<ViewChangeSchooolOrder> res = new ViewChangeSchooolOrderRepository()
                .GetChangeInList(schoolId, searcher, GetSearchForStudents(searcher.Key));

            //2.组合转入列表数据
            return new PageResult<ChangeSchoolOrderListResponse>
            {
                CurrentPage = res.CurrentPage,
                PageSize = res.PageSize,
                TotalData = res.TotalData,
                Data = GetChangeSchoolOrderListResponseList(res.Data)
            };
        }

        /// <summary>
        /// 获取搜索学生Id
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="key">搜索关键词</param>
        /// <returns>满足条件的学生Id</returns>
        private static List<long> GetSearchForStudents(string key)
        {
            List<long> studentIds = new List<long>();

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.Trim();

                //根据关键词(名字、手机号=LinkMobile)
                studentIds = StudentService
                    .GetStudentsByKey(key)
                    .Select(x => x.StudentId)
                    .ToList();
            }

            return studentIds;
        }

        /// <summary>
        ///  获取转校列表数据组合
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-11</para>
        /// </summary>
        /// <param name="data">转校列表数据</param>
        /// <returns>获取转校列表数据组合</returns>
        private static List<ChangeSchoolOrderListResponse> GetChangeSchoolOrderListResponseList(IReadOnlyList<ViewChangeSchooolOrder> data)
        {
            //1.获取学生Id
            List<long> studentIds = data.Select(x => x.StudentId).ToList();

            //2.获取学生基本信息
            List<TblCstStudent> studentInfos = StudentService.GetStudentByIds(studentIds).Result;

            //3.获取所有校区
            List<SchoolResponse> allSchool = new OrgService().GetAllSchoolList();

            //4.组合数据
            List<ChangeSchoolOrderListResponse> res = new List<ChangeSchoolOrderListResponse>();
            foreach (var item in data)
            {
                TblCstStudent studentInfo = studentInfos.FirstOrDefault(x => x.StudentId == item.StudentId) ?? new TblCstStudent();
                SchoolResponse schoolInfo = allSchool.FirstOrDefault(x => x.SchoolId == item.InSchoolId) ?? new SchoolResponse();

                var model = new ChangeSchoolOrderListResponse
                {
                    OrderId = item.RefundOrderId,
                    Amount = item.Amount + item.TransFromBalance, //金额=退费的课程费用-扣费项+转出余额
                    CreateTime = item.CreateTime,
                    StudentId = item.StudentId,
                    StudentName = studentInfo.StudentName ?? string.Empty,
                    LinkMobile = studentInfo.ContactPersonMobile.Split(',').FirstOrDefault() ?? string.Empty,
                    StudentNo = studentInfo.StudentNo ?? string.Empty,
                    OrderNo = item.OrderNo,
                    OrderStatus = (OrderStatus)item.OrderStatus,
                    OrderStatusName = EnumName.GetDescription(typeof(OrderStatusTransferResponse), item.OrderStatus),
                    SchoolName = schoolInfo.SchoolName ?? string.Empty,
                    Time = item.OutDate
                };

                res.Add(model);
            }

            return res;
        }
        #endregion

        #region Cancel 撤销

        /// <summary>
        /// 撤销转校订单
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="cancelUserId">操作者用户Id</param>
        /// <param name="cancelUserName">作废操作人名称</param>
        /// <param name="cancelRemark">取消备注</param>
        /// <exception cref="Exception">系统异常</exception>
        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, _studentId.ToString()))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        FlowBack(orderId, cancelRemark, OrderStatus.Cancel, cancelUserId, cancelUserName, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }
                }
            }
            return Task.CompletedTask;
        }
        #endregion

        #region FinanceConfirm 财务确认
        /// <summary>
        /// 财务确认
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="request">财务确认提交的数据</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：订单为空
        /// 异常ID：5,异常描述：此状态不能执行此操作
        /// </exception>
        public void FinanceConfirm(ChangeSchoolOrderConfirmRequest request)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, _studentId.ToString()))
            {
                var refundOrder = base._tblOdrRefundOrderRepository.Value.Load(request.OrderId);
                if (refundOrder == null)
                {
                    throw new BussinessException(ModelType.Default, 1);
                }
                if (refundOrder.OrderStatus != (int)OrderStatus.WaitPay)
                {
                    //此状态不能执行此操作
                    throw new BussinessException(ModelType.Order, 5);
                }

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    var tblOdrRefundChangeSchoolOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundChangeSchoolOrderRepository, TblOdrRefundChangeSchoolOrder>();
                    var tblOdrRefundOrderCostRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderCostRepository, TblOdrRefundOrderCost>();
                    var tblOdrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();

                    //更新支付账号信息
                    this.UpdateRefundPay(request.TransferAccounts, request.OrderId, unitOfWork);

                    //更新票据
                    var refundChangeSchoolOrder = tblOdrRefundChangeSchoolOrderRepository.Load(request.OrderId);
                    refundChangeSchoolOrder.ReceiptStatus = request.ReceiptStatus;
                    tblOdrRefundChangeSchoolOrderRepository.Update(refundChangeSchoolOrder);

                    //更新扣款项
                    tblOdrRefundOrderCostRepository.ClearRefundOrderCostByRefundOrderId(request.OrderId);
                    this.AddRefundOrderCost(request.Cost, request.OrderId, unitOfWork);

                    decimal costItemTotal = this.GetCostItemTotal(request.Cost);

                    refundOrder.OrderStatus = (int)OrderStatus.Confirm;//财务确认

                    //更新退费金额=退费金额+原扣款金额-新扣款金额
                    refundOrder.Amount = refundOrder.Amount + refundOrder.TotalDeductAmount - costItemTotal;
                    //更新扣款项金额
                    refundOrder.TotalDeductAmount = costItemTotal;

                    tblOdrRefundOrderRepository.Update(refundOrder);

                    //变更交易数据
                    ChangeSchoolOrderTrade orderTrade = new ChangeSchoolOrderTrade(
                        refundOrder,
                        unitOfWork);

                    FrozeTradeService trade = new FrozeTradeService(orderTrade, unitOfWork);
                    trade.UpdateNewTradingInfo();

                    //添加操作日志
                    this.WriteOperationLog(request.OrderId, OperationFlowStatus.Confirm, request.OperatorId, request.OperatorName, unitOfWork);
                }
            }
        }

        /// <summary>
        /// 修改退费订单-支付信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-13</para>
        /// </summary>
        /// <param name="request">账户信息</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void UpdateRefundPay(TransferAccountsRequest request, long refundOrderId, UnitOfWork unitOfWork)
        {
            if (request == null)
            {
                return;
            }

            TblOdrRefundPayRepository repository = unitOfWork.GetCustomRepository<TblOdrRefundPayRepository, TblOdrRefundPay>();

            TblOdrRefundPay refundPay = repository.GetRefundOrderIdByRefundPay(refundOrderId);

            //退费订单-支付信息不为空更改支付信息
            refundPay.BankCardNo = request.BankCardNumber ?? string.Empty;
            refundPay.BankName = request.IssuingBank ?? string.Empty;
            refundPay.BankUserName = request.AccountName ?? string.Empty;

            repository.Update(refundPay);
        }
        #endregion

        #region Receive 转校接收
        /// <summary>
        /// 转校接收(取消/拒绝)
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="request">转校接收提交的数据</param>
        /// <exception cref="Exception">系统异常</exception>
        public void Receive(ChangeSchoolOrderReceiveRequest request)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_AMSSCHOOLSTUDENT, _schoolId, _studentId.ToString()))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        switch (request.ReceiveStatus)
                        {
                            case ChangeSchoolOrderReceiveStatus.Receive://接收
                                this.ConfirmReceive(request.OrderId, request.OperatorId, request.OperatorName, unitOfWork);
                                break;
                            case ChangeSchoolOrderReceiveStatus.Refuse: //拒绝
                                this.FlowBack(request.OrderId, request.Remark, OrderStatus.Refuse, request.OperatorId, request.OperatorName, unitOfWork);
                                break;
                            default:
                                break;
                        }
                        unitOfWork.CommitTransaction();
                    }
                    catch
                    {
                        unitOfWork.RollbackTransaction();
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// 确认接收
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="operatorId">操作人Id</param>
        /// <param name="operatorName">操作人名称</param>
        /// <param name="unitOfWork">工作单元</param>
        private void ConfirmReceive(long orderId, string operatorId, string operatorName, UnitOfWork unitOfWork)
        {
            var tblOdrRefundOrderRepository = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();

            //1.获取订单
            var refundOrder = tblOdrRefundOrderRepository.Load(orderId);

            //2.校验订单
            this.ConfirmReceiveVerify(refundOrder);

            //3.更新订单
            refundOrder.OrderStatus = (int)OrderStatus.Paid;
            tblOdrRefundOrderRepository.Update(refundOrder);

            //3.1 A校区转出B校区接收数据对接
            //A校区转出到B校区
            ChangeSchoolOrderTrade orderTrade = new ChangeSchoolOrderTrade(refundOrder, unitOfWork);
            FrozeTradeService trade = new FrozeTradeService(orderTrade, unitOfWork);
            trade.TradeComplete();

            //A校区转入到B校区数据
            ChangeSchoolInOrderTrade orderTrade2 = new ChangeSchoolInOrderTrade(refundOrder, unitOfWork);
            TradeService trade2 = new TradeService(orderTrade2, unitOfWork);
            trade2.Trade();

            //4.写入操作记录
            this.WriteOperationLog(refundOrder.RefundOrderId, OperationFlowStatus.Finish, operatorId, operatorName, unitOfWork);
        }

        /// <summary>
        /// 转校确认接收确认校验
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-14</para>
        /// </summary>
        /// <param name="refundOrder">订单信息</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：订单为空
        /// 异常ID：7,异常描述：订单已交接
        /// 异常ID：6,异常描述：订单状态不可交接
        /// </exception>
        private void ConfirmReceiveVerify(TblOdrRefundOrder refundOrder)
        {
            if (refundOrder == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            }

            //判断订单是否已交接
            if (refundOrder.OrderStatus == (int)OrderStatus.Paid || refundOrder.OrderStatus == (int)OrderStatus.Refuse)
            {
                throw new BussinessException(ModelType.Order, 7);//订单已交接
            }

            //判断订单是否可交接
            if (refundOrder.OrderStatus != (int)OrderStatus.Confirm)
            {
                throw new BussinessException(ModelType.Order, 6);
            }
        }

        #endregion

        #region 获取退费订单信息
        /// <summary>
        /// 获取退费订单信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-14</para>
        /// </summary>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns></returns>
        public static TblOdrRefundOrder GetRefundOrder(long refundOrderId)
        {
            return new TblOdrRefundOrderRepository().Load(refundOrderId);
        }
        #endregion

        /// <summary>
        /// 流程回退 
        /// <para>描    述：用于财务撤销转校/用于他校拒绝接收</para>
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-01-04</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="cancelRemark">原因</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="cancelUserId">取消人Id</param>
        /// <param name="cancelUserName">取消人名称</param>
        private void FlowBack(
            long orderId, string cancelRemark, OrderStatus orderStatus,
            string cancelUserId, string cancelUserName, UnitOfWork unitOfWork)
        {
            TblOdrRefundOrderRepository tblOdrRefundOrderRepository = unitOfWork
                .GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();

            //1.获取订单
            TblOdrRefundOrder refundOrder = tblOdrRefundOrderRepository.Load(orderId);

            //2.校验订单合法性
            FlowBackVerify(refundOrder);

            //3.更新转校订单信息
            this.UpdateFlowBackRefundOrder(
                tblOdrRefundOrderRepository, refundOrder, cancelRemark,
                orderStatus, cancelUserId, cancelUserName);

            //4.回退转校前的课次
            this.FlowBackChangeSchoolLesson(refundOrder.RefundOrderId, unitOfWork);

            //5.更新报名的课程订单状态
            this.UpdateFlowBackEnrollOrderItemStatus(orderId, unitOfWork);

            //6.资金返回
            ChangeSchoolOrderTrade orderTrade = new ChangeSchoolOrderTrade(
                refundOrder,
                unitOfWork);
            FrozeTradeService trade = new FrozeTradeService(orderTrade, unitOfWork);
            trade.Cancel();

            //7.操作日记
            this.WriteOperationLog(
                refundOrder.RefundOrderId,
                this.GetOperationFlowStatusByOrderStatus(orderStatus),
                cancelUserId,
                cancelUserName,
                unitOfWork,
                cancelRemark);

            //8.更新学生课次
            new StudentService(this._schoolId).UpdateStudentStatusById(this._studentId, unitOfWork);
        }

        /// <summary>
        /// 更新报名的课程订单状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void UpdateFlowBackEnrollOrderItemStatus(long orderId, UnitOfWork unitOfWork)
        {
            TblOdrRefundOrdeEnrollRepository tblOdrRefundOrdeEnrollRepository = unitOfWork
                .GetCustomRepository<TblOdrRefundOrdeEnrollRepository, TblOdrRefundOrdeEnroll>();
            TblOdrEnrollOrderItemRepository tblOdrEnrollOrderItemRepository = unitOfWork
                .GetCustomRepository<TblOdrEnrollOrderItemRepository, TblOdrEnrollOrderItem>();

            //找到相关转校订单信息然后循环更新报名的课程订单状态
            List<TblOdrRefundOrdeEnroll> refundOrdeEnrolls = tblOdrRefundOrdeEnrollRepository
                .GetRefundOrderByOrderEnroll(orderId);
            List<TblOdrEnrollOrderItem> enrollOrderItems = tblOdrEnrollOrderItemRepository
                .GetByEnrollOrderItemId(refundOrdeEnrolls.Select(x => x.EnrollOrderItemId))
                .Result;
            foreach (var enrollOrderItem in enrollOrderItems)
            {
                enrollOrderItem.Status = (int)OrderItemStatus.Enroll;
                tblOdrEnrollOrderItemRepository.Update(enrollOrderItem);
            }
        }

        /// <summary>
        /// 回退转校前的课次
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="refundOrderId">转校订单Id</param>
        /// <param name="unitOfWork">工作单元</param>
        private void FlowBackChangeSchoolLesson(long refundOrderId, UnitOfWork unitOfWork)
        {
            //实例化课次生产者
            ChangeSchoolLessonCreator lessonCreator = new ChangeSchoolLessonCreator(refundOrderId, unitOfWork);
            LessonService lessonService = new LessonService(unitOfWork);
            lessonService.Create(lessonCreator);
        }

        /// <summary>
        /// 更新回退转校订单信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="tblOdrRefundOrderRepository">转校订单仓储</param>
        /// <param name="refundOrder">转校订单信息</param>
        /// <param name="cancelRemark">取消原因</param>
        /// <param name="orderStatus">取消状态</param>
        /// <param name="cancelUserId">取消人Id</param>
        /// <param name="cancelUserName">取消人名称</param>
        private void UpdateFlowBackRefundOrder(
            TblOdrRefundOrderRepository tblOdrRefundOrderRepository,
            TblOdrRefundOrder refundOrder,
            string cancelRemark,
            OrderStatus orderStatus,
            string cancelUserId,
            string cancelUserName)
        {
            refundOrder.OrderStatus = (int)orderStatus;
            refundOrder.CancelDate = DateTime.Now;
            refundOrder.CancelRemark = cancelRemark;
            refundOrder.CancelUserId = cancelUserId;
            //更新订单信息
            tblOdrRefundOrderRepository.Update(refundOrder);
        }

        /// <summary>
        /// 根据订单状态获取操作日志状态
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        private OperationFlowStatus GetOperationFlowStatusByOrderStatus(OrderStatus orderStatus)
        {
            OperationFlowStatus operationFlowStatus = default(OperationFlowStatus);
            switch (orderStatus)
            {
                case OrderStatus.Refuse:
                    operationFlowStatus = OperationFlowStatus.Refuse;//已拒绝
                    break;
                default:
                    operationFlowStatus = OperationFlowStatus.Cancel;//已取消
                    break;
            }

            return operationFlowStatus;
        }

        /// <summary>
        /// 校验当前状态是否可以撤销
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-15</para>
        /// </summary>
        /// <param name="refundOrder">转校订单信息</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,异常描述：传入的转校订单信息为空
        /// 异常ID：5,异常描述：当前状态不能撤销
        /// </exception>
        private void FlowBackVerify(TblOdrRefundOrder refundOrder)
        {
            if (refundOrder == null)
            {
                throw new BussinessException(ModelType.Default, 1);
            };

            //待确认、待接收才可以撤销
            if (refundOrder.OrderStatus != (int)OrderStatus.WaitPay
                && refundOrder.OrderStatus != (int)OrderStatus.Confirm)
            {
                //当前状态不能撤销
                throw new BussinessException(ModelType.Order, 5);
            }
        }
    }
}
