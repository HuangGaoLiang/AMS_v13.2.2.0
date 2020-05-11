using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：余额退费办理
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class BalanceRefundOrderServices : BaseRefundOrderService
    {
        private readonly Lazy<ViewBalanceRefundOrderRepository> _viewBalanceRepository = new Lazy<ViewBalanceRefundOrderRepository>();//余额退费视图仓储



        /// <summary>
        /// 描述：实例化指定校区的余额退费办理
        /// <para>作    者：</para>
        /// <para>创建时间：</para>
        /// </summary>
        /// <param name="schoolId"></param>
        public BalanceRefundOrderServices(string schoolId) : this(schoolId, 0)
        {

        }

        /// <summary>
        /// 余额退费办理实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        public BalanceRefundOrderServices(string schoolId, long studentId) : base(schoolId, studentId)
        {
        }


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
        /// 获取余额退费订单详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>订单详情</returns>
        public override IOrderDetailResponse GetOrderDetail(long orderId, string companyId)
        {
            var result = new BalanceRefundOrderDetailResponse();
            //获取余额退费订单信息
            var refundInfo = _viewBalanceRepository.Value.GetBalanceRefundByOrderId(orderId);
            if (refundInfo != null)
            {
                result = Mapper.Map<BalanceRefundOrderDetailResponse>(refundInfo);
                //获取其他费用(扣款合计)
                var refundAmountTypeList = CostService.GetCosts(Dto.TypeCode.LEAVE_CLASS_FEE);
                var costList = _tblOdrRefundOrderCost.Value.GetrefundOrderIdByCost(orderId).Select(x => new CostDetail
                {
                    CostName = refundAmountTypeList.FirstOrDefault(k => k.CostId == x.CostId)?.CostName,
                    Amount = x.Amount
                }).ToList();
                result.CostList = costList;

                //获取余额退班的附件
                var attchmentList = new AttchmentService(base._schoolId).GetAttchList(orderId, AttchmentType.ORDER_BALANCE_REFUND);
                result.AttachmentList = attchmentList;
            }

            return result;
        }

        /// <summary>
        /// 获取余额退费列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="request">余额退费请求对象</param>
        /// <returns></returns>
        public static PageResult<BalanceRefundOrderListResponse> GetBalanceRefundList(BalanceRefundListSearchRequest request)
        {
            ViewBalanceRefundOrderRepository repository = new ViewBalanceRefundOrderRepository();
            var result = new PageResult<BalanceRefundOrderListResponse>() { Data = new List<BalanceRefundOrderListResponse>() };
            //获取余额退费列表
            var query = repository.GetBalanceRefundList(request);
            if (query != null && query.Data != null && query.Data.Count > 0)
            {
                result.Data = Mapper.Map<List<BalanceRefundOrderListResponse>>(query.Data);
                result.CurrentPage = query.CurrentPage;
                result.PageSize = query.PageSize;
                result.TotalData = query.TotalData;
            }
            return result;
        }

        /// <summary>
        /// 办理前获取学生的余额信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="iRequest">退费订单办理前获取详情的参数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>退费订单办理前的详情信息</returns>
        public override IRefundOrderTransactDetailReponse GetTransactDetail(IRefundOrderTransacDetailtRequest iRequest, string companyId)
        {
            BalanceRefundOrderTransactDetailResponse result = new BalanceRefundOrderTransactDetailResponse();
            //获取学生相关信息
            var studentInfo = new StudentService(this._schoolId).GetStudent(this._studentId);
            if (studentInfo != null)
            {
                List<GuardianRequest> guardianList = studentInfo.ContactPerson;
                result = new BalanceRefundOrderTransactDetailResponse()
                {
                    StudentId = studentInfo.StudentId,
                    StudentName = studentInfo.StudentName,
                    StudentNo = studentInfo.StudentNo,
                    Sex = EnumName.GetDescription(typeof(SexEnum), studentInfo.Sex),
                    Mobile = studentInfo.LinkMobile,
                    GuardianName = guardianList?[0].GuardianName,
                    Age = Age.GetAgeByDate(studentInfo.Birthday, DateTime.Now),
                    Amount = studentInfo.Balance
                };
            }
            return result;
        }

        /// <summary>
        /// 获取余额退费订单的打印收据详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="orderId">余额退费订单id</param>
        /// <returns>余额退费打印信息</returns>
        public BalanceRefundReceiptDetailResponse GetPrintReceiptDetail(long orderId)
        {
            var result = new BalanceRefundReceiptDetailResponse();
            //获取余额退费订单信息
            var refundInfo = _viewBalanceRepository.Value.GetBalanceRefundByOrderId(orderId);
            if (refundInfo != null)
            {
                //获取余额退费的打印序号
                var printNo = PrintCounterService.Print(base._schoolId, PrintBillType.BalanceRefund);
                //获取其他费用(扣款合计)
                var refundAmountTypeList = CostService.GetCosts(Dto.TypeCode.BALANCE_FEE);
                var costList = _tblOdrRefundOrderCost.Value.GetrefundOrderIdByCost(orderId).Select(x => new CostDetail
                {
                    CostName = refundAmountTypeList.FirstOrDefault(k => k.CostId == x.CostId)?.CostName,
                    Amount = x.Amount
                }).ToList();
                result = new BalanceRefundReceiptDetailResponse
                {
                    OrderNo = refundInfo.OrderNo,
                    StudentId = refundInfo.StudentId,
                    PrintNo = printNo,
                    CreatorName = refundInfo.CreatorName,
                    Amount = refundInfo.Amount,
                    TotalDeductAmount = refundInfo.TotalDeductAmount,
                    RealRefundAmount = refundInfo.RealRefundAmount,
                    RefundDate = refundInfo.RefundDate,
                    Remark = refundInfo.Remark,
                    CostList = costList,
                    SchoolName = OrgService.GetSchoolBySchoolId(refundInfo.SchoolId)?.SchoolName
                };
            }

            return result;
        }

        /// <summary>
        /// 余额退费办理
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="iRequest">退费订单办理数据</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 15. 转出余额不足
        /// 23. 该学生已经没有余额可退
        /// </exception>
        public override void Transact(IRefundOrderTransactRequest iRequest, string companyId)
        {
            var refundRequest = iRequest as BalanceRefundOrderRequest;

            //1、数据校验
            CheckBeforeTransact(refundRequest);

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.BeginTransaction(System.Data.IsolationLevel.Snapshot);

                    //2、保存退费订单信息
                    var refundOrderId = IdGenerator.NextId();
                    TblOdrRefundOrder refundOrder = AddRefundOrder(unitOfWork, refundRequest, refundOrderId).Result;

                    //3、保存余额退费的其他费用信息
                    AddRefundOrderCost(unitOfWork, refundRequest, refundOrderId);

                    //4、保存余额退费上传的附件
                    AddAttachment(unitOfWork, refundRequest.AttachmentUrlList, refundOrderId);

                    //5、保存余额退费的支付信息
                    AddRefundPay(unitOfWork, refundRequest, refundOrderId);

                    //6、资金调用
                    BalanceOrderTrade orderTrade = new BalanceOrderTrade(refundOrder, "余额退费");
                    TradeService trade = new TradeService(orderTrade, unitOfWork);
                    trade.Trade();

                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-05</para>
        /// </summary>
        /// <param name="refundRequest">余额退费请求对象</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 15. 转出余额不足
        /// 23. 该学生已经没有余额可退
        /// </exception>
        private void CheckBeforeTransact(BalanceRefundOrderRequest refundRequest)
        {
            //1、获取学生的余额信息
            decimal balance = new WalletService(this._schoolId, this._studentId, null).Balance;

            //检查用户是否还有余额
            if (balance == 0)
            {
                throw new BussinessException((byte)ModelType.Order, 23);
            }
            //检查用户余额是否足够转出
            if (refundRequest.RefundAmount > balance)
            {
                throw new BussinessException((byte)ModelType.Order, 15);
            }
        }

        /// <summary>
        /// 添加余额退费的其他扣费项
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <param name="refundRequest">余额退费请求对象</param>
        /// <param name="refundOrderId">退费订单Id</param>
        private void AddRefundOrderCost(UnitOfWork unitOfWork, BalanceRefundOrderRequest refundRequest, long refundOrderId)
        {
            if (!refundRequest.OtherCostRequest.Any())
            {
                return;
            }
            var rep = unitOfWork.GetCustomRepository<TblOdrRefundOrderCostRepository, TblOdrRefundOrderCost>();
            List<TblOdrRefundOrderCost> refundOrderCosts = refundRequest.OtherCostRequest.Select(m => new TblOdrRefundOrderCost
            {
                Amount = m.CostAmount,
                SchoolId = this._schoolId,
                CostId = m.CostId,
                CreateTime = refundRequest.CreateTime,
                RefundClassCostId = IdGenerator.NextId(),
                RefundOrderId = refundOrderId
            }).ToList();

            rep.Add(refundOrderCosts);
        }

        /// <summary>
        /// 保存余额退费上传的附件
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <param name="attachmentUrlList">附件地址列表</param>
        /// <param name="refundOrderId">退费订单Id</param>
        private void AddAttachment(UnitOfWork unitOfWork, List<AttchmentAddRequest> attachmentUrlList, long refundOrderId)
        {
            if (attachmentUrlList == null || attachmentUrlList.Count == 0)
            {
                return;
            }
            attachmentUrlList.ForEach(a => { a.AttchmentType = AttchmentType.ORDER_BALANCE_REFUND; a.BusinessId = refundOrderId; });
            new AttchmentService(this._schoolId, unitOfWork).Add(attachmentUrlList);
        }

        /// <summary>
        /// 保存余额退费的支付信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <param name="refundRequest">余额退费请求对象</param>
        /// <param name="refundOrderId">退费订单Id</param>
        private void AddRefundPay(UnitOfWork unitOfWork, BalanceRefundOrderRequest refundRequest, long refundOrderId)
        {
            var rep = unitOfWork.GetCustomRepository<TblOdrRefundPayRepository, TblOdrRefundPay>();
            TblOdrRefundPay refundPay = new TblOdrRefundPay
            {
                RefundPayId = IdGenerator.NextId(),
                SchoolId = this._schoolId,
                RefundOrderId = refundOrderId,
                RefundType = (int)RefundType.RefundToParent,
                BankName = refundRequest.BankName,
                BankCardNo = refundRequest.BankCardNo,
                BankUserName = refundRequest.BankUserName,
                Remark = refundRequest.Reason,
                CreateTime = refundRequest.CreateTime
            };
            rep.Add(refundPay);
        }

        /// <summary>
        /// 保存退费订单信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="unitOfWork">事务单元</param>
        /// <param name="refundRequest">余额退费请求对象</param>
        /// <param name="refundOrderId">退费订单Id</param>
        /// <returns></returns>
        private async Task<TblOdrRefundOrder> AddRefundOrder(UnitOfWork unitOfWork, BalanceRefundOrderRequest refundRequest, long refundOrderId)
        {
            var rep = unitOfWork.GetCustomRepository<TblOdrRefundOrderRepository, TblOdrRefundOrder>();
            TblOdrRefundOrder refundOrder = new TblOdrRefundOrder
            {
                RefundOrderId = refundOrderId,
                SchoolId = this._schoolId,
                StudentId = this._studentId,
                OrderNo = base.CreateOrderNo(refundOrderId),
                OrderType = (int)OrderTradeType.BalanceOrder,
                TotalDeductAmount = this.GetOtherCostAmount(refundRequest.OtherCostRequest),//扣款项统计 refundRequest.TotalDeductAmount,
                Amount = refundRequest.RefundAmount,
                OrderStatus = (int)OrderStatus.Paid,
                CancelUserId = string.Empty,
                CancelDate = null,
                CancelRemark = string.Empty,
                CreatorId = refundRequest.CreatorId,
                CreatorName = refundRequest.CreatorName,
                CreateTime = refundRequest.CreateTime
            };
            await rep.AddTask(refundOrder);
            return refundOrder;
        }

        /// <summary>
        /// 统计其他扣费扣款项金额
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="costRequests">扣费信息对象</param>
        /// <returns>扣费总金额</returns>
        private decimal GetOtherCostAmount(List<CostRequest> costRequests)
        {
            if (costRequests != null && costRequests.Any())
            {
                return costRequests.Sum(x => x.CostAmount);
            }
            return 0;
        }

        /// <summary>
        /// 余额退费订单作废
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="cancelUserId">作废人Id</param>
        /// <param name="cancelUserName">作废人名称</param>
        /// <param name="cancelRemark">作废理由</param>
        /// <returns>无</returns>
        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            throw new NotImplementedException();
        }
    }
}
