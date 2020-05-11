using AMS.Anticorrosion.HRS;
using AMS.Core;
using AMS.Core.Locks;
using AMS.Dto;
using AMS.Models;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using AMS.Storage.Repository.Orders;
using AutoMapper;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述: 定金订单服务
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class DepositOrderService : BaseOrderService
    {
        /// <summary>
        /// 订金仓储
        /// </summary>
        private readonly Lazy<TblOdrDepositOrderRepository> _odrDepositOrderRepository = new Lazy<TblOdrDepositOrderRepository>();

        /// <summary>
        /// 订金订单
        /// </summary>
        protected override string OrderNoTag => "D";

        /// <summary>
        /// 校区编号
        /// </summary>
        private readonly string _schoolId;

        /// <summary>
        /// 构造函数
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2019-02-18</para>
        /// </summary>
        /// <param name="schoolId">校区编号</param>
        public DepositOrderService(string schoolId)
        {
            this._schoolId = schoolId;
        }

        #region GetList  获取定金表列
        /// <summary>
        /// 根据条件查询定金表列
        /// <para>作     者:Huang GaoLiang </para> 
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="seacher">定金的查询条件</param>
        /// <returns>返回订金分页列表</returns>
        public PageResult<DepositOrderListResponse> GetList(DepositOrderListSearchRequest seacher)
        {
            PageResult<ViewDepositOrder> orderList = new ViewDepositOrderRepository().GetOrderByWhere(seacher);
            PageResult<DepositOrderListResponse> list = Mapper.Map<PageResult<ViewDepositOrder>, PageResult<DepositOrderListResponse>>(orderList);
            return list;
        }

        #endregion


        /// <summary>
        /// 
        /// <para>作     者: </para> 
        /// <para>创建时间:</para>
        /// </summary>
        /// <param name="searcher"></param>
        /// <returns></returns>
        public override LeaveSchoolOrderListResponse GetOrderList(IOrderListSearchRequest searcher)
        {
            throw new NotImplementedException();
        }

        #region 根据收银人Id获取学生定金信息
        /// <summary>
        /// 根据收银人Id获取学生定金信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="payee">收银人Id</param>
        /// <returns>定金订单信息列表</returns>
        internal async Task<List<TblOdrDepositOrder>> GetDepositOrderByPayee(string payee)
        {
            return await _odrDepositOrderRepository.Value.GetDepositOrderByPayee(this._schoolId, payee);
        }
        #endregion

        #region GetDetail 获取定金详情信息
        /// <summary>
        /// 获取定金详情信息
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="depositOrderId">订金编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订金详情数据</returns>
        public DepositOrderDetailResponse GetDetail(long depositOrderId, string companyId)
        {
            DepositOrderDetailResponse res = (DepositOrderDetailResponse)GetOrderDetail(depositOrderId, companyId);

            FinOrderHandoverResponse r = new OrderHandoverService(_schoolId).GetFinOrderHandoverDetailByOrderId(depositOrderId, OrderTradeType.DepositOrder);
            res.FinOrderHandoverList = r ?? new FinOrderHandoverResponse();
            return res;

        }
        #endregion

        #region GetPrintOrderDetail 订金打印
        /// <summary>
        /// 获取定金详情信息
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-11-15 </para>
        /// </summary>
        /// <param name="depositOrderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回订金详情数据</returns>
        /// <exception cref="BussinessException">
        /// 异常ID：5,数据异常
        /// </exception>
        public DepositOrderDetailResponse GetPrintOrderDetail(long depositOrderId, string companyId)
        {
            DepositOrderDetailResponse order = (DepositOrderDetailResponse)GetOrderDetail(depositOrderId, companyId);
            order.SchoolName = new OrgService().GetAllSchoolList().FirstOrDefault(m => m.SchoolId == order.SchoolId)?.SchoolName;
            order.PrintNumber = PrintCounterService.Print(_schoolId, PrintBillType.DepositOrderBill);

            // 根据学生编号获取学生信息
            StudentDetailResponse studentInfo = new StudentService(_schoolId).GetStudent(order.StudentId);
            if (studentInfo == null)
            {
                throw new BussinessException((byte)ModelType.Cash, 5);
            }
            order.StudentName = studentInfo.StudentName;
            order.StudentNo = studentInfo.StudentNo;
            order.LinkMobile = studentInfo.LinkMail;
            order.Age = studentInfo.Age;
            return order;
        }
        #endregion

        #region Add 添加订金
        /// <summary>
        /// 添加订金
        /// <para>作     者：Huang GaoLiang </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="request">订金充值实体</param>
        /// <param name="payeeId">收银人编号</param>
        /// <param name="payeeName">收银人</param>
        /// <returns>返回订金编号</returns>

        public string Add(DepositOrderAddRequest request, string payeeId, string payeeName)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_DEPOSITORDERADD, this._schoolId, request.StudentId.ToString()))
            {
                // 1、数据合法性校验
                Verification(request);

                long depositOrderId = IdGenerator.NextId();

                // 2、准备数据
                TblOdrDepositOrder odrDepositOrder = this.SetOdrDepositOrder(request, payeeId, payeeName, depositOrderId);

                // 3、资金调用
                DepositOrderTrade orderTrade = new DepositOrderTrade(odrDepositOrder);

                // 4、添加事务
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        TradeService tradeService = new TradeService(orderTrade, unitOfWork);
                        tradeService.Trade();

                        unitOfWork.GetCustomRepository<TblOdrDepositOrderRepository, TblOdrDepositOrder>().AddTask(odrDepositOrder).Wait();

                        unitOfWork.CommitTransaction();
                        return depositOrderId.ToString();

                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 订金充值
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="request">订金充值实体</param>
        /// <param name="payeeId">收银人编号</param>
        /// <param name="payeeName">收银人姓名</param>
        /// <param name="depositOrderId">订金编号</param>
        /// <returns>返回订金实体信息</returns>
        private TblOdrDepositOrder SetOdrDepositOrder(DepositOrderAddRequest request, string payeeId, string payeeName, long depositOrderId)
        {
            TblOdrDepositOrder odrDepositOrder = new TblOdrDepositOrder
            {
                DepositOrderId = depositOrderId,
                StudentId = request.StudentId,
                SchoolId = _schoolId,
                OrderNo = CreateOrderNo(depositOrderId),
                PayDate = DateTime.Now,
                UsesType = (int)request.UsesType,
                Amount = request.Amount,
                PayeeId = payeeId,
                Payee = payeeName,
                PayType = (int)request.PayType,
                Remark = request.Remark,
                OrderStatus = (int)OrderStatus.Paid, //-1订单取消, 0待付款,1已付款/正常
                CancelUserId = "",
                CancelRemark = "",
                CreateTime = DateTime.Now
            };
            return odrDepositOrder;
        }

        /// <summary>
        /// 订金充值校验
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="request">订金充值请求数据</param>
        /// <exception cref="BussinessException">
        /// 异常ID：1,定金充值金额必须大于
        /// 异常ID：2,预报名定金最多可以输入2000元
        /// 异常ID：3,游学营定金最多可输入4万元
        /// 异常ID：4,请选择付款方式
        /// </exception>
        private void Verification(DepositOrderAddRequest request)
        {
            // 1、订金金额
            if (request.Amount <= 0)
            {
                throw new BussinessException((byte)ModelType.Cash, 1);
            }

            // 2、订金用途
            if (request.UsesType == UsesType.SignUp && request.Amount > 2000)
            {
                throw new BussinessException((byte)ModelType.Cash, 2);
            }

            // 游学营订金充值不能大于4万
            else if (request.UsesType == UsesType.StudyCamp && request.Amount > 40000)
            {
                throw new BussinessException((byte)ModelType.Cash, 3);
            }

            // 3、付款方式
            if (request.PayType <= 0)
            {
                throw new BussinessException((byte)ModelType.Cash, 4);
            }
        }

        #endregion

        #region Cancel 作废定金
        /// <summary>
        /// 作废定金
        /// <para>作     者::Huang GaoLiang  </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="orderId">订金编号</param>
        /// <param name="cancelUserId">作废人操作编号</param>
        /// <param name="cancelUserName">作废人名称</param>
        /// <param name="cancelRemark">作废原因</param>
        /// <exception cref="BussinessException">
        /// 异常ID：5,数据异常
        /// 异常ID：8,余额不足，不能作废
        /// </exception>
        public override Task Cancel(long orderId, string cancelUserId, string cancelUserName, string cancelRemark)
        {
            lock (LocalThreadLock.GetLockKeyName(LockKeyNames.LOCK_DEPOSITORDERCANCEL, this._schoolId, orderId.ToString()))
            {
                // 根据订金编号查询订金信息
                TblOdrDepositOrder odrDepositOrder = _odrDepositOrderRepository.Value.GetOdrDepositOrder(orderId);

                if (odrDepositOrder == null)
                {
                    throw new BussinessException((byte)ModelType.Cash, 5);
                }

                // 1、数据校验
                OdrDepositOrderCancelVerification(odrDepositOrder);

                // 2、准备数据
                odrDepositOrder.OrderStatus = (int)OrderStatus.Cancel;
                odrDepositOrder.CancelDate = DateTime.Now;
                odrDepositOrder.CancelUserId = cancelUserId;
                odrDepositOrder.CancelUserName = cancelUserName;
                odrDepositOrder.CancelRemark = cancelRemark;

                // 3、作废之后，余额扣除掉相应的金额 
                bool isWalletSufficient = WalletService.IsWalletSufficient(_schoolId, odrDepositOrder.StudentId, odrDepositOrder.Amount);

                if (!isWalletSufficient)
                {
                    throw new BussinessException((byte)ModelType.Cash, 8);
                }

                DepositOrderTrade orderTrade = new DepositOrderTrade(odrDepositOrder);

                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.BeginTransaction();

                        // 1、资金交易
                        TradeService tradeService = new TradeService(orderTrade, unitOfWork);
                        tradeService.Invalid();

                        // 2、订金
                        unitOfWork.GetCustomRepository<TblOdrDepositOrderRepository, TblOdrDepositOrder>().UpdateTask(odrDepositOrder).Wait();

                        // 3、收款交接 
                        new OrderHandoverService(_schoolId).DeleteHandleOver(orderId, OrderTradeType.DepositOrder, unitOfWork);

                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollbackTransaction();
                        throw ex;
                    }
                }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 订金作废逻辑判断
        /// <para>作     者::Huang GaoLiang  </para>
        /// <para>创建时间:2018-10-30 </para>
        /// </summary>
        /// <param name="depositOrder">订金信息</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：5,数据异常
        /// 异常ID：6,余额不足，不能作废
        /// </exception>
        private static void OdrDepositOrderCancelVerification(TblOdrDepositOrder depositOrder)
        {
            //作废逻辑：
            //1、已交接和已作废的数据不能再作废，不显示【作废】的按钮。
            //2、作废的时候判断学生的余额够不够作废，不够则不能作废。

            if (depositOrder == null)
            {
                throw new BussinessException((byte)ModelType.Cash, 5);
            }

            // 已交接
            if (depositOrder.OrderStatus == Convert.ToInt32(OrderStatus.Finish))
            {
                throw new BussinessException((byte)ModelType.Cash, 6);
            }

            // 已作废
            if (depositOrder.OrderStatus == Convert.ToInt32(OrderStatus.Cancel))
            {
                throw new BussinessException((byte)ModelType.Cash, 6);
            }
        }

        /// <summary>
        /// 根据订金编号获取订金详情
        /// <para>作     者::Huang GaoLiang  </para>
        /// <para>创建时间:2018-11-10 </para>
        /// </summary>
        /// <param name="orderId">订金编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns>返回获取订金详情</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：7,数据异常
        /// </exception>
        public override IOrderDetailResponse GetOrderDetail(long orderId, string companyId)
        {
            TblOdrDepositOrder odrDepositOrder = _odrDepositOrderRepository.Value.GetOdrDepositOrderById(orderId).Result;

            DepositOrderDetailResponse info = Mapper.Map<TblOdrDepositOrder, DepositOrderDetailResponse>(odrDepositOrder);

            // 根据学生编号查询学生信心
            StudentDetailResponse s = new StudentService(_schoolId).GetStudent(info.StudentId);
            if (s == null)
            {
                throw new BussinessException((byte)ModelType.SignUp, 7);
            }

            info.StudentName = s.StudentName;
            info.SexName = s.SexName;
            info.HeadFaceUrl = s.HeadFaceUrl;
            info.Age = s.Age;
            info.IDNumber = s.IDNumber;
            info.IDType = s.IDType;
            info.IDTypeName = s.IdTypeName;
            info.ContactPerson = s.ContactPerson;
            return info;
        }

        #endregion

    }
}
