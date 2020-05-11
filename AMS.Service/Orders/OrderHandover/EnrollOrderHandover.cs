using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：报名订单未交接
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class EnrollOrderHandover : BaseOrderHandover, IOrderHandover
    {
        private readonly string _schoolId;                                                                                                                                                                                                                              //校区Id
        private readonly Lazy<TblOdrEnrollOrderRepository> _enrollRepository = new Lazy<TblOdrEnrollOrderRepository>();                                                                                       //报班仓储
        private readonly Lazy<ViewFinOrderHandoverDetailSummaryRepository> _detailSummaryRepository = new Lazy<ViewFinOrderHandoverDetailSummaryRepository>();      //订单交接核对明细汇总仓储 

        /// <summary>
        /// 报名订单未交接实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-28</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        public EnrollOrderHandover(string schoolId)
        {
            _schoolId = schoolId;
        }

        #region 获取报班未交接的相关信息
        /// <summary>
        /// 获取报班未交接的统计列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <returns>订单未交接汇总列表</returns>
        public List<OrderUnHandoverCountResponse> GetUnHandCountList()
        {
            return _detailSummaryRepository.Value.GetEnrollHandoverSummary(_schoolId);
        }

        /// <summary>
        /// 获取报班未交接的列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-15</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>订单未交接列表</returns>
        public List<OrderUnHandoverListResponse> GetUnHandList(string personalId)
        {
            List<OrderUnHandoverListResponse> result = new List<OrderUnHandoverListResponse>();
            //获取招生专员对应的学生报名信息
            var enrollOrderInfoList = new EnrollOrderService(_schoolId).GetEnrollOrderByCreatorId(personalId).Result.Where(a => a.OrderStatus == (int)OrderStatus.Paid).ToList();
            if (enrollOrderInfoList != null && enrollOrderInfoList.Count > 0)
            {
                //获取学生信息
                var studentInfoList = StudentService.GetStudentByIds(enrollOrderInfoList.Select(a => a.StudentId)).Result;
                //获取招生专员对应的订单交接核对明细信息
                var handoverDetailList = _detailRepository.Value.GetDetailsByPersonalId(_schoolId, personalId).Result;
                //过滤报班订单已交接的数据
                var enrollOrderList = enrollOrderInfoList.Where(a => !handoverDetailList.Any(b => b.OrderId == a.EnrollOrderId && b.OrderTradeType == (int)OrderTradeType.EnrollOrder)
                                        || (handoverDetailList.Exists(c => a.EnrollOrderId == c.OrderId && c.OrderTradeType == (int)OrderTradeType.EnrollOrder && c.HandoverStatus != (int)HandoverStatus.Handover)));
                foreach (var item in enrollOrderList)
                {
                    //学生对应的学生信息
                    var studentInfo = studentInfoList.FirstOrDefault(s => s.StudentId == item.StudentId);
                    OrderUnHandoverListResponse handoverModel = new OrderUnHandoverListResponse()
                    {
                        OrderId = item.EnrollOrderId,
                        OrderNo = item.OrderNo,
                        StudentNo = studentInfo?.StudentNo,
                        StudentName = studentInfo?.StudentName,
                        PayAmount = item.PayAmount,
                        TradeType = (int)OrderTradeType.EnrollOrder,
                        TradeTypeName = EnumName.GetDescription(typeof(OrderTradeType), (int)OrderTradeType.EnrollOrder),//报班
                        Status = (int)TransferStatus.UnTransfer,
                        StatusName = EnumName.GetDescription(typeof(TransferStatus), (int)TransferStatus.UnTransfer),
                        Remark = item.Remark,
                        CreateTime = item.CreateTime
                    };
                    //报单订单对应的订单交接核对明细信息
                    var handoverDetail = handoverDetailList.FirstOrDefault(h => h.PersonalId == item.CreateId && h.OrderId == item.EnrollOrderId);
                    if (handoverDetail != null)
                    {
                        handoverModel.Status = handoverDetail.HandoverStatus.Value;
                        handoverModel.StatusName = EnumName.GetDescription(typeof(HandoverStatus), handoverDetail.HandoverStatus);
                    }
                    result.Add(handoverModel);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取未交接的报班订单列表（问题单与未交接）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-21</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <returns>收款交接订单添加的实体列表</returns>
        public List<OrderHandleAddResponse> GetUnHandleAddList(string personalId)
        {
            var result = new List<OrderHandleAddResponse>();
            //获取招生专员对应的学生报名信息
            var enrollOrderInfoList = new EnrollOrderService(_schoolId).GetEnrollOrderByCreatorId(personalId).Result.Where(a => a.OrderStatus == (int)OrderStatus.Paid).ToList();
            if (enrollOrderInfoList != null && enrollOrderInfoList.Count > 0)
            {
                //获取招生专员对应的订单交接核对明细信息
                var handoverDetailList = _detailRepository.Value.GetDetailsByPersonalId(_schoolId, personalId).Result;
                //获取报班订单问题单/未交接的数据
                var enrollOrderList = enrollOrderInfoList.Where(a => !handoverDetailList.Any(b => b.OrderId == a.EnrollOrderId && b.OrderTradeType == (int)OrderTradeType.EnrollOrder)
                                        || (handoverDetailList.Exists(c => a.EnrollOrderId == c.OrderId && c.OrderTradeType == (int)OrderTradeType.EnrollOrder && c.HandoverStatus == (int)HandoverStatus.Problematic)));
                if (enrollOrderList != null && enrollOrderList.Any())
                {
                    result = enrollOrderList.Select(a => new OrderHandleAddResponse
                    {
                        OrderId = a.EnrollOrderId,
                        OrderTradeType = (int)OrderTradeType.EnrollOrder,
                        CreateTime = a.CreateTime
                    }).ToList();
                }
            }
            return result;
        }
        #endregion

        #region 获取下一报班订单
        /// <summary>
        /// 获取下一需要交接的订单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-19</para>
        /// </summary>
        /// <param name="personalId">招生专员Id</param>
        /// <param name="orderId">已核对的订单Id</param>
        /// <returns>收款交接订单添加的实体</returns>
        public OrderHandleAddResponse GetNextHandoverOrder(string personalId, long orderId)
        {
            OrderHandleAddResponse result = new OrderHandleAddResponse();
            var enrollOrderInfoList = new EnrollOrderService(_schoolId).GetEnrollOrderByCreatorId(personalId).Result.Where(a => a.OrderStatus == (int)OrderStatus.Paid).ToList();
            if (enrollOrderInfoList != null && enrollOrderInfoList.Count > 0)
            {
                //获取时间最早的一笔未交接的报名订单，下一步进行核对
                var enrollOrderInfo = enrollOrderInfoList.OrderBy(o => o.CreateTime).FirstOrDefault(a => a.EnrollOrderId != orderId);
                if (enrollOrderInfo != null)
                {
                    result.OrderId = enrollOrderInfo.EnrollOrderId;
                    result.OrderTradeType = (int)OrderTradeType.EnrollOrder;
                }
            }
            return result;
        }
        #endregion

        #region 获取未交接的报班订单详情信息
        /// <summary>
        /// 根据订单Id获取报名信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// <param name="orderId">订单Id</param>
        /// </summary>
        /// <returns>订单未交接详情</returns>
        public OrderUnHandoverDetailResponse GetUnHandoverDetail(long orderId)
        {
            var result = new OrderUnHandoverDetailResponse();
            var orderInfo = _enrollRepository.Value.GetOrderById(orderId);//报名订单信息            
            if (orderInfo != null)
            {
                //学生信息
                var studentInfo = new StudentService(orderInfo.SchoolId).GetStudent(orderInfo.StudentId);
                result = new OrderUnHandoverDetailResponse
                {
                    OrderId = orderInfo.EnrollOrderId,
                    PersonalId = orderInfo.CreateId,
                    PersonalName = orderInfo.CreateName,
                    StudentName = studentInfo?.StudentName,
                    StudentNo = studentInfo?.StudentNo,
                    OrderType = (int)OrderTradeType.EnrollOrder,
                    OrderTypeName = EnumName.GetDescription(typeof(OrderTradeType), (int)OrderTradeType.EnrollOrder),
                    CreateTime = DateTime.Now
                };
            }
            return result;
        }
        #endregion

        #region 获取报班收款交易表信息
        /// <summary>
        /// 获取已核对的未交接订单明细
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-17</para>
        /// </summary>
        /// <param name="handoverDetails">已核对的未交接订单明细</param>
        /// <returns>订单类型的交接核对信息</returns>
        public OrderHandoverTradeResponse GetHandoverTradeList(IEnumerable<TblFinOrderHandoverDetail> handoverDetails)
        {
            return base.GetOrderHandoverTrade(handoverDetails, OrderTradeType.EnrollOrder);
        }
        #endregion

        #region 报班未交接单核对和问题单
        /// <summary>
        /// 对未交接订单进行核对
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接数据对象</param>
        /// <returns>订单交接核对明细Id</returns>
        /// <exception cref="AMS.Core.BusinessException">
        /// 异常ID：
        /// 13. 此单信息核对有误
        /// 25. 订单已作废，不可核对
        /// </exception>
        public long Handle(OrderHandleAddRequest request)
        {
            //1.获取报班订单交接核对明细
            var handoverDetailList = _detailRepository.Value.GetDetailsByOrderId(new List<long> { request.OrderId });
            //获取报名的订单交接核对明细，如果已存在，则直接返回订单交接核对明细Id
            if (handoverDetailList != null && handoverDetailList.Any(a => a.HandoverStatus == (int)HandoverStatus.Checked || a.HandoverStatus == (int)HandoverStatus.Handover))
            {
                return handoverDetailList.FirstOrDefault().OrderHandoverDetailId;
            }

            //2.数据校验 检查付款方式和金额跟订单是否一致，如不一致，则标志问题单
            var orderInfo = _enrollRepository.Value.GetOrderById(request.OrderId);//报班订单信息
            CheckData(request, x => orderInfo.OrderStatus == (int)OrderStatus.Cancel, 25);//订单状态检查
            CheckData(request, x => x.PayType != orderInfo.PayType, 13);//付款方式
            CheckData(request, x => (x.UseBalanceAmount ?? 0) != orderInfo.UseBalance, 13);//与订单的使用余额是否一致
            CheckData(request, x => !x.PayAmount.HasValue || (x.PayAmount.HasValue && x.PayAmount.Value != orderInfo.PayAmount), 13);//金额跟订单是否一致

            //3.如有问题单，则将其更新为已核对状态
            var handoverDetail = handoverDetailList.FirstOrDefault(a => a.HandoverStatus == (int)HandoverStatus.Problematic);
            if (handoverDetail != null)
            {
                return UpdateUnhandover(request, handoverDetail);
            }

            //4.订单核对
            return AddEnrollUnhandover(request, HandoverStatus.Checked, orderInfo);
        }

        /// <summary>
        /// 将报名订单设为问题单
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接数据对象</param>
        /// <returns>订单交接核对明细Id</returns>
        public long MarkOrderProblem(OrderHandleAddRequest request)
        {
            //获取报名的订单交接核对明细，如果已存在，则直接返回订单交接核对明细Id
            var handoverDetail = _detailRepository.Value.GetDetailsByOrderId(new List<long> { request.OrderId });
            if (handoverDetail != null && handoverDetail.Count > 0)
            {
                return handoverDetail.FirstOrDefault().OrderHandoverDetailId;
            }
            //获取订单id对应的报名订单信息
            var orderInfo = _enrollRepository.Value.GetOrderById(request.OrderId);
            //添加有问题的未交接核对信息
            return AddEnrollUnhandover(request, HandoverStatus.Problematic, orderInfo);
        }
        #endregion

        #region 未交接订单明细新增/更新操作
        /// <summary>
        /// 添加已核对的未交接核对信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="request">核对未交接数据对象</param>
        /// <param name="status">订单状态</param>
        /// <param name="orderInfo">订单信息对象</param>
        /// <returns>订单交接核对明细Id</returns>
        private long AddEnrollUnhandover(OrderHandleAddRequest request, HandoverStatus status, TblOdrEnrollOrder orderInfo)
        {
            var entity = new TblFinOrderHandoverDetail
            {
                OrderHandoverDetailId = IdGenerator.NextId(),
                OrderId = orderInfo.EnrollOrderId,
                OrderNo = orderInfo.OrderNo,
                OrderTradeType = request.OrderTradeType,
                PayType = request.PayType,
                PayAmount = request.PayAmount.Value,
                HandoverStatus = (int)status,
                PersonalId = request.PersonalId,
                PayDate = request.PayDate,
                SchoolId = request.SchoolId,
                UseBalanceAmount = request.UseBalanceAmount ?? 0,
                CreateTime = DateTime.Now,
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName,
                StudentId = orderInfo.StudentId,
                Remark = orderInfo.Remark,
                TotalDiscountFee = orderInfo.TotalDiscountFee
            };
            _detailRepository.Value.Add(entity);

            return entity.OrderHandoverDetailId;
        }
        #endregion        

        #region 更新报班订单状态
        /// <summary>
        /// 更新报班订单状态
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-26</para>
        /// </summary>
        /// <param name="orderIdList">订单Id列表</param>
        /// <param name="status">订单状态</param>
        public void UpdateOrderStatus(List<long> orderIdList, OrderStatus status)
        {
            _enrollRepository.Value.UpdateOrderStatus(orderIdList, status).Wait();
        }
        #endregion
    }
}
