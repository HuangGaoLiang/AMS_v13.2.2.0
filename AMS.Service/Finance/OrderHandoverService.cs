using System;
using AMS.Dto;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Public.PageExtensions;
using System.Collections.Generic;
using System.Linq;
using AMS.Storage;
using AMS.Storage.Models;
using AutoMapper;
using System.Threading.Tasks;
using AMS.Core;
using AMS.Anticorrosion.HRS;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：订单交接业务类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class OrderHandoverService
    {
        private readonly string _schoolId;                                                                                                                                                                                                                         //校区Id
        private readonly Dictionary<OrderTradeType, IOrderHandover> _orderHandoverList;                                                                                                                                       //订单类型实例字典
        private readonly Lazy<TblFinOrderHandoverRepository> _repository = new Lazy<TblFinOrderHandoverRepository>();                                                                                  //订单交接仓储
        private Lazy<TblFinOrderHandoverDetailRepository> _detailRepository = new Lazy<TblFinOrderHandoverDetailRepository>();                                                                    //订单交接明细仓储
        private readonly Lazy<TblDatAttchmentRepository> _datAttchmentRepository = new Lazy<TblDatAttchmentRepository>();                                                                           //订单交接附件仓储
        private readonly Lazy<ViewFinOrderHandoverRepository> _viewHandoverReqpository = new Lazy<ViewFinOrderHandoverRepository>();                                                  //订单交接仓储
        private readonly Lazy<ViewFinOrderHandoverDetailSummaryRepository> _viewDetailRepository = new Lazy<ViewFinOrderHandoverDetailSummaryRepository>();         //订单交接明细仓储 

        #region Constructor

        /// <summary>
        /// 订单交接业务类实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public OrderHandoverService(string schoolId)
        {
            this._schoolId = schoolId;

            _orderHandoverList = new Dictionary<OrderTradeType, IOrderHandover>
            {
                { OrderTradeType.EnrollOrder, new EnrollOrderHandover(this._schoolId) },
                { OrderTradeType.DepositOrder, new DepositOrderHandover(this._schoolId) }
            };
        }

        #endregion

        #region 获取交接的业务订单类实例
        /// <summary>
        /// 获取交接的业务订单类
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="orderTradeType">订单交易类型</param>
        /// <returns>业务订单接口</returns>
        private IOrderHandover GetOrderHandover(int orderTradeType)
        {
            return _orderHandoverList[(OrderTradeType)orderTradeType];
        }
        #endregion

        #region 获取未交接订单的相关信息

        /// <summary>
        /// 获取校区未交接订单汇总列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <returns>订单未交接汇总列表</returns>
        public List<OrderUnHandoverCountResponse> GetUnHandCountList()
        {
            List<OrderUnHandoverCountResponse> result = new List<OrderUnHandoverCountResponse>();
            foreach (IOrderHandover iOrder in _orderHandoverList.Values)
            {
                var list = iOrder.GetUnHandCountList();
                if (list != null && list.Count > 0)
                {
                    result.AddRange(list);
                }
            }
            result = result.GroupBy(g => g.PersonalId).Select(a => new OrderUnHandoverCountResponse
            {
                SchoolId = a.Min(p => p.SchoolId),
                PersonalId = a.Key,
                PersonalName = a.Min(p => p.PersonalName),
                Amount = a.Sum(p => p.Amount),
                UnHandoverNumber = a.Sum(p => p.UnHandoverNumber)
            }).ToList();
            result = result.OrderByDescending(a => a.UnHandoverNumber).ToList();
            return result;
        }

        /// <summary>
        /// 根据招生专员Id、页数和页大小获取校区未交接订单列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>订单未交接分页列表</returns>
        public PageResult<OrderUnHandoverListResponse> GetUnHandList(string personalId, int pageIndex, int pageSize)
        {
            PageResult<OrderUnHandoverListResponse> result = new PageResult<OrderUnHandoverListResponse>() { Data = new List<OrderUnHandoverListResponse>() };
            List<OrderUnHandoverListResponse> handoverList = new List<OrderUnHandoverListResponse>();
            foreach (IOrderHandover iOrder in _orderHandoverList.Values)
            {
                handoverList.AddRange(iOrder.GetUnHandList(personalId));
            }
            var resultList = handoverList.OrderBy(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            result.CurrentPage = pageIndex;
            result.PageSize = pageSize;
            result.TotalData = handoverList.Count;
            result.Data = resultList;
            return result;
        }

        /// <summary>
        /// 根据招生专员Id、订单Id和订单类型获取校区未交接订单列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderTradeType">订单类型</param>
        /// <returns>收款交接要添加的订单列表</returns>
        public List<OrderHandleAddResponse> GetOrderHandleAddList(string personalId, long orderId, int orderTradeType)
        {
            List<OrderHandleAddResponse> result = new List<OrderHandleAddResponse>();
            foreach (IOrderHandover iOrder in _orderHandoverList.Values)
            {
                var list = iOrder.GetUnHandleAddList(personalId);
                if (list.Count > 0)
                {
                    result.AddRange(list);
                }
            }
            result = result.Where(a => a.OrderId != orderId).OrderBy(a => a.CreateTime).ToList();
            return result;
        }

        /// <summary>
        /// 获取校区未交接订单列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="handoverDetails">订单交接核对明细列表</param>
        /// <returns>订单类型的交接核对信息列表</returns>
        private List<OrderHandoverTradeResponse> GetHandoverTradeListByTradeType(IEnumerable<TblFinOrderHandoverDetail> handoverDetails)
        {
            List<OrderHandoverTradeResponse> handoverList = new List<OrderHandoverTradeResponse>();
            foreach (IOrderHandover iOrder in _orderHandoverList.Values)
            {
                var detail = iOrder.GetHandoverTradeList(handoverDetails);
                if (detail != null)
                {
                    handoverList.Add(detail);
                }
            }
            return handoverList;
        }

        #endregion

        #region 获取收款交接表信息

        /// <summary>
        /// 根据招生专员Id获取收款交接表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>收款交接表信息</returns>
        public OrderHandoverReportResponse GetHandoverListByPersonalId(string personalId)
        {
            //获取招生专员对应的订单交接核对明细
            var handoverDetails = _detailRepository.Value.GetDetailsByPersonalId(this._schoolId, personalId).Result.Where(a => a.HandoverStatus == (int)HandoverStatus.Checked);
            OrderHandoverReportResponse result = GetHandoverList(handoverDetails);

            //获取招生专员信息
            result.PersonalName = EmployeeService.GetByEmployeeId(personalId)?.EmployeeName;
            result.HandoverDate = DateTime.Now;
            return result;
        }

        /// <summary>
        /// 根据收款交接Id获取收款交接表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="handoverId">订单交接Id</param>
        /// <returns>收款交接表信息</returns>
        public OrderHandoverReportResponse GetHandoverListByHandoverId(long handoverId)
        {
            //获取收款交接信息
            var handoverInfo = _repository.Value.GetByHandoverId(handoverId);
            if (handoverInfo != null)
            {
                //获取招生专员对应的订单交接核对明细
                var handoverDetails = _detailRepository.Value.GetDetailsByHandoverId(handoverId).Result;
                //获取收款交接明细的汇总信息
                OrderHandoverReportResponse result = GetHandoverList(handoverDetails);
                result.DayIncomeAmout = handoverInfo.DayIncomeAmout;
                result.HandoverDate = handoverInfo.HandoverDate;
                result.HandoverUrl = handoverInfo.HandoverUrl;
                result.InBankAmount = handoverInfo.InBankAmount;
                result.PersonalName = handoverInfo.PersonalName;
                result.ReceiptNumber = handoverInfo.ReceiptNumber;
                result.RecipientUrl = handoverInfo.RecipientUrl;
                result.Remark = handoverInfo.Remark;
                return result;
            }

            return new OrderHandoverReportResponse();
        }

        /// <summary>
        /// 获取收款交接汇总信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="handoverDetails">收款交接明细信息列表</param>
        /// <returns>收款交接表信息</returns>
        private OrderHandoverReportResponse GetHandoverList(IEnumerable<TblFinOrderHandoverDetail> handoverDetails)
        {
            OrderHandoverReportResponse report = new OrderHandoverReportResponse();
            if (handoverDetails != null && handoverDetails.Any())
            {
                //获取按订单类型汇总的订单交接核对明细（支付日期等于创建日期的已核对的订单）
                var handoverDetailsByTrade = handoverDetails.Where(a => a.CreateTime.Date == a.PayDate.Date);
                var tradeList = GetHandoverTradeListByTradeType(handoverDetailsByTrade);
                report.TradeList = tradeList;
                report.TradeTotalAmount = tradeList.Sum(a => a.TotalAmount);

                //获取按日期汇总的订单交接核对明细(往日已收款&今日入系统(本月)的已核对的订单)
                var handoverDetailsByCurrentMonth = handoverDetails.Where(a => a.PayDate.Date != a.CreateTime.Date && a.PayDate.Month == DateTime.Now.Month);
                var currentMonthList = GetHandoverListByDay(handoverDetailsByCurrentMonth);
                report.CurrentMonthList = currentMonthList.OrderBy(a => a.PayDate).ToList();
                report.CurrentMonthTotalAmount = currentMonthList.Sum(a => a.TotalAmount);

                //获取按日期汇总的订单交接核对明细(往日已收款&今日入系统(往月)的已核对的订单)
                var handoverDetailsByLastMonth = handoverDetails.Where(a => a.PayDate.Date != a.CreateTime.Date && a.PayDate.Month != DateTime.Now.Month);
                var lastMonthList = GetHandoverListByDay(handoverDetailsByLastMonth);
                report.LastMonthList = lastMonthList.OrderBy(a => a.PayDate).ToList();
                report.LastMonthTotalAmount = lastMonthList.Sum(a => a.TotalAmount);
                //合计金额
                report.TotalAmount = report.TradeTotalAmount + report.CurrentMonthTotalAmount + report.LastMonthTotalAmount;
                //总记录数
                report.TotalRecord = tradeList.Sum(a => a.HandoverNumber) + currentMonthList.Sum(a => a.HandoverNumber) + lastMonthList.Sum(a => a.HandoverNumber);

                //获取按订单类型汇总的订单交接核对明细(所有已核对的订单)
                var tradeAllList = GetHandoverTradeListByTradeType(handoverDetails);
                var tradeSummaryList = tradeAllList.Select(a => new OrderHandoverTradeSummaryResponse
                {
                    OrderTradeType = a.OrderTradeType,
                    OrderTradeTypeName = a.OrderTradeTypeName,
                    Cash = a.Cash,
                    OtherAmount = a.OtherAmount,
                    PayAmount = a.PayAmount,
                    TransferAmount = a.TransferAmount,
                    //合计=现钞+刷卡/微信/支付宝+转账+其他
                    TotalAmount = a.Cash + a.PayAmount + a.TransferAmount + a.OtherAmount
                }).ToList();
                report.TradeSummaryList = tradeSummaryList;
                report.TradeSummaryTotalAmount = tradeSummaryList.Sum(a => a.TotalAmount);
                report.DayIncomeAmout = tradeSummaryList.Sum(a => a.Cash);//收现钞=所有当前已核对的订单的支付方式是现钞的金额的之和
            }

            return report;
        }

        /// <summary>
        /// 获取订单交接核对订单明细（按日期汇总）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="handoverDetails">订单交接核对订单明细</param>
        /// <returns>财务交接订单信息列表</returns>
        private List<OrderHandoverDaySummaryResponse> GetHandoverListByDay(IEnumerable<TblFinOrderHandoverDetail> handoverDetails)
        {
            if (handoverDetails != null && handoverDetails.Any())
            {
                var result = new List<OrderHandoverDaySummaryResponse>();
                //按日期进行汇总
                var dayList = handoverDetails.Select(a => a.PayDate.Date).Distinct();
                //无现金支付（刷卡/支付宝/微信）
                List<int> payNoCashType = new List<int>()
                {
                    (int)PayType.AliPay,
                    (int)PayType.Pos,
                    (int)PayType.WxPay
                };
                foreach (var day in dayList)
                {
                    var handoverDayDetails = handoverDetails.Where(a => a.PayDate == day);
                    if (handoverDayDetails != null && handoverDayDetails.Any())
                    {
                        var entity = new OrderHandoverDaySummaryResponse()
                        {
                            PayDate = day,
                            Cash = handoverDayDetails.Where(a => a.PayType == (int)PayType.Cash).Sum(b => b.PayAmount),
                            BalanceAmount = handoverDayDetails.Sum(a => a.UseBalanceAmount),
                            CouponAmount = handoverDayDetails.Sum(a => a.TotalDiscountFee),
                            HandoverNumber = handoverDayDetails.Count(),
                            OtherAmount = handoverDayDetails.Where(a => a.PayType == (int)PayType.Other).Sum(b => b.PayAmount),
                            PayAmount = handoverDayDetails.Where(a => payNoCashType.Contains(a.PayType)).Sum(b => b.PayAmount),
                            TransferAmount = handoverDayDetails.Where(a => a.PayType == (int)PayType.BankTransfer).Sum(b => b.PayAmount)
                        };
                        //合计 = 现钞 + 刷卡 / 微信 / 支付宝 + 转账 + 其他 + 使用余额 + 使用奖学金
                        entity.TotalAmount = entity.Cash + entity.BalanceAmount + entity.CouponAmount + entity.OtherAmount + entity.PayAmount + entity.TransferAmount;
                        result.Add(entity);
                    }
                }
                return result;
            }
            return new List<OrderHandoverDaySummaryResponse>();
        }

        #endregion

        #region 生成收款交接表

        /// <summary>
        /// 生成收款交接表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="request">收款交接信息请求对象</param>
        /// <returns>无</returns>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 21. 系统不存在该招生专员的收款交接信息
        /// 26. 交接单发生变化，请重新交接
        /// </exception>
        public async Task SaveReport(OrderHandoverRequest request)
        {
            //获取收款交接明细信息
            var details = _detailRepository.Value.GetDetailsByPersonalId(request.SchoolId, request.PersonalId).Result.Where(a => a.HandoverStatus == (int)HandoverStatus.Checked);

            //1.数据校验 
            CheckReport(request, details);

            //2.生成收款交接信息
            TblFinOrderHandover entity = AddOrderHandover(request);

            //3.收款交接明细的交接日期
            await _detailRepository.Value.UpdateDetailsByHandoverId(entity);

            //4.更新订单状态为已完成
            UpdateOrderStatus(details);
        }

        /// <summary>
        /// 生成收款交接单之前进行数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-25</para>
        /// </summary>
        /// <param name="request">收款交接信息请求对象</param>
        /// <param name="details">收款交接明细信息</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 21. 系统不存在该招生专员的收款交接信息
        /// 26. 交接单发生变化，请重新交接
        /// </exception>
        private void CheckReport(OrderHandoverRequest request, IEnumerable<TblFinOrderHandoverDetail> details)
        {
            //是否存在收款交接明细信息
            if (!details.Any())
            {
                throw new BussinessException((byte)ModelType.Order, 21);
            }
            //检查收款交接单的总记录数与总金额数是否有问题
            if (details.Count() != request.TotalRecord || details.Sum(a => a.PayAmount + a.UseBalanceAmount + a.TotalDiscountFee) != request.TotalAmount)
            {
                throw new BussinessException((byte)ModelType.Order, 26);
            }
        }

        /// <summary>
        /// 添加收款交接信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-25</para>
        /// </summary>
        /// <param name="request">收款交接信息请求对象</param>
        /// <returns>订单交接核对信息</returns>
        private TblFinOrderHandover AddOrderHandover(OrderHandoverRequest request)
        {
            var entity = new TblFinOrderHandover
            {
                OrderHandoverId = IdGenerator.NextId(),
                SchoolId = request.SchoolId,
                PersonalId = request.PersonalId,
                PersonalName = request.PersonalName,
                DayIncomeAmout = request.DayIncomeAmout,
                InBankAmount = request.InBankAmount,
                ReceiptNumber = request.ReceiptNumber,
                HandoverDate = request.HandoverDate,
                RecipientUrl = request.RecipientUrl,
                HandoverUrl = request.HandoverUrl,
                Remark = request.Remark,
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName,
                CreateTime = request.CreateTime
            };
            _repository.Value.Add(entity);
            return entity;
        }


        /// <summary>
        /// 更新订单状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="details">订单交接核对明细列表</param>
        public void UpdateOrderStatus(IEnumerable<TblFinOrderHandoverDetail> details)
        {
            foreach (var key in _orderHandoverList.Keys)
            {
                var orderIdList = details.Where(a => a.OrderTradeType == (int)key).Select(x => x.OrderId).ToList();
                if (orderIdList != null && orderIdList.Count > 0)
                {
                    _orderHandoverList[key].UpdateOrderStatus(orderIdList, OrderStatus.Finish);
                }
            }
        }

        #endregion

        #region 收款交接记录

        /// <summary>
        /// 获取校区已交接订单列表
        /// 只允许查询最多一个月时间段
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="request">订单已交接查询条件</param>
        /// <returns>订单已交接分页列表</returns>
        public PageResult<OrderHandoverListResponse> GetHandList(OrderHandoverListSearchRequest request)
        {
            var result = new PageResult<OrderHandoverListResponse>() { Data = new List<OrderHandoverListResponse>() };
            var query = _viewHandoverReqpository.Value.GetOrderHandoverList(request);
            if (query != null && query.Data != null && query.Data.Count > 0)
            {
                result.CurrentPage = query.CurrentPage;
                result.PageSize = query.PageSize;
                result.TotalData = query.TotalData;
                result.Data = Mapper.Map<List<OrderHandoverListResponse>>(query.Data);
            }
            return result;
        }

        /// <summary>
        /// 根据请求参数获取对应的订单交接明细列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="request">订单交接明细请求对象</param>
        /// <returns>订单交接明细信息分页列表</returns>
        public PageResult<OrderHandoverDetailListResponse> GetHandDetailList(OrderHandoverDetailRequest request)
        {
            var result = _viewDetailRepository.Value.GetDetailsByHandoverId(request);
            if (result != null && result.Data != null && result.Data.Count > 0)
            {
                //获取校区的所有学生信息
                var studentInfoList = StudentService.GetStudentByIds(result.Data.Select(a => a.StudentId).Distinct()).Result;
                result.Data.ForEach(a =>
                {
                    var studentInfo = studentInfoList.FirstOrDefault(s => s.StudentId == a.StudentId);//学生信息
                    a.StudentNo = studentInfo?.StudentNo;
                    a.StudentName = studentInfo?.StudentName;
                    a.OrderTradeTypeName = EnumName.GetDescription(typeof(OrderTradeType), a.OrderTradeType);
                });
                return result;
            }
            return new PageResult<OrderHandoverDetailListResponse>();
        }

        #endregion

        #region 获取未交接的订单详情信息
        /// <summary>
        /// 根据订单Id获取对应订单未交接详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderTradeType">订单交易类型</param>
        /// <returns>订单未交接详情</returns>
        public OrderUnHandoverDetailResponse GetUnHandoverDetail(long orderId, int orderTradeType)
        {
            return GetOrderHandover(orderTradeType).GetUnHandoverDetail(orderId);
        }
        #endregion

        #region 未交接单核对和问题单

        /// <summary>
        /// 对未交接订单进行核对
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接的请求对象</param>
        /// <returns>收款交接要添加的订单信息</returns>
        public OrderHandleAddResponse Handle(OrderHandleAddRequest request)
        {
            //进行核对
            long handoverDetailId = this.GetOrderHandover(request.OrderTradeType).Handle(request);

            //上传附件并返回下一订单
            return AddAttachmentAndReturnNextOrder(handoverDetailId, request);
        }

        /// <summary>
        /// 将订单设为问题单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接的请求对象</param>
        /// <returns>收款交接要添加的订单信息</returns>
        public OrderHandleAddResponse MarkOrderProblem(OrderHandleAddRequest request)
        {
            //进行核对
            long handoverDetailId = this.GetOrderHandover(request.OrderTradeType).MarkOrderProblem(request);

            //上传附件并返回下一订单
            return AddAttachmentAndReturnNextOrder(handoverDetailId, request);
        }

        /// <summary>
        /// 上传附件并返回下一订单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="handoverDetailId">添加收款未交接Id</param>
        /// <param name="request">核对未交接的请求对象</param>
        /// <returns>收款交接要添加的订单信息</returns>
        private OrderHandleAddResponse AddAttachmentAndReturnNextOrder(long handoverDetailId, OrderHandleAddRequest request)
        {
            //3、保存余额退费上传的附件
            AddAttachment(request.AttachmentUrlList, handoverDetailId);

            //先获取报名订单
            return GetNextHandoverOrder(request.PersonalId, request.OrderId);
        }

        /// <summary>
        /// 获取下一需要交接的订单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="orderId">已核对的订单Id</param>
        /// <returns>收款交接要添加的订单信息</returns>
        private OrderHandleAddResponse GetNextHandoverOrder(string personalId, long orderId)
        {
            OrderHandleAddResponse result = new OrderHandleAddResponse();
            foreach (IOrderHandover iOrder in _orderHandoverList.Values)
            {
                //获取下一要交接的订单
                result = iOrder.GetNextHandoverOrder(personalId, orderId);
                if (result.OrderId != 0)
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 保存收款交接上传的附件
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="attachmentUrlList">附件对象集合</param>
        /// <param name="handoverId">收款交接单Id</param>
        private void AddAttachment(List<AttchmentAddRequest> attachmentUrlList, long handoverId)
        {
            if (attachmentUrlList == null || attachmentUrlList.Count == 0)
            {
                return;
            }
            attachmentUrlList.ForEach(a => { a.AttchmentType = AttchmentType.ORDER_HAND_OVER; a.BusinessId = handoverId; });
            new AttchmentService(this._schoolId).Add(attachmentUrlList);
        }

        #endregion

        #region 订单作废，删除对应收款交接信息
        /// <summary>
        /// 订单作废之后，对已核对/问题的收款交接进行删除
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-19</para>
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="tradeType">订单类型</param>
        /// <param name="unitOfWork">工作单元</param>
        public void DeleteHandleOver(long orderId, OrderTradeType tradeType, UnitOfWork unitOfWork = null)
        {
            if (unitOfWork != null)
            {
                _detailRepository = new Lazy<TblFinOrderHandoverDetailRepository>(() => { return unitOfWork.GetCustomRepository<TblFinOrderHandoverDetailRepository, TblFinOrderHandoverDetail>(); });
            }
            _detailRepository.Value.DeleteDetailByOrderId(this._schoolId, orderId, tradeType);
        }
        #endregion

        #region 检查招生专员是否存在已核对的收款交接信息
        /// <summary>
        /// 检查招生专员是否存在已核对的收款交接信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：
        /// 21. 系统不存在该招生专员的收款交接信息
        /// </exception>
        public void IsExistOrderHandover(string personalId)
        {
            var handoverDetails = _detailRepository.Value.GetDetailsByPersonalId(this._schoolId, personalId).Result;
            if (!handoverDetails.Any(a => a.HandoverStatus == (int)HandoverStatus.Checked))
            {
                throw new BussinessException((byte)ModelType.Order, 21);
            }
        }
        #endregion       

        #region GetFinOrderHandoverDetailByOrderId 根据订单编号获取已交接的订单明细

        /// <summary>
        /// 根据订单编号获取已交接的订单明细
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="orderType">订单类型</param>
        /// <returns>返回已交接的订单明细</returns>
        internal FinOrderHandoverResponse GetFinOrderHandoverDetailByOrderId(long orderId, OrderTradeType orderType)
        {
            FinOrderHandoverResponse finOrderHandover = new FinOrderHandoverResponse();

            // 1、获取已交接的收款详情信息
            TblFinOrderHandoverDetail d = _detailRepository.Value.GetDetailById(orderId, orderType);
            if (d != null && d.HandoverStatus == (int)HandoverStatus.Handover)
            {
                //2、附件
                List<TblDatAttchment> urls = _datAttchmentRepository.Value.LoadList(m => m.SchoolId == _schoolId).OrderBy(m => m.CreateTime).ToList();
                finOrderHandover.OrderHandoverId = d.OrderHandoverDetailId;
                finOrderHandover.CreateTime = d.CreateTime;
                finOrderHandover.PayDate = d.PayDate;
                finOrderHandover.CreatorName = d.CreatorName;
                finOrderHandover.HandoverDate = d.HandoverDate;
                finOrderHandover.Url = urls.Where(y => y.BusinessId == d.OrderHandoverDetailId).Select(z => z.Url).ToList();
                return finOrderHandover;
            }
            return finOrderHandover;
        }
        #endregion
    }
}
