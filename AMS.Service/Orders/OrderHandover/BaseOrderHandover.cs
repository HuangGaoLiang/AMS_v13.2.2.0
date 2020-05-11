using AMS.Core;
using AMS.Dto;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：订单交接抽象基类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public abstract class BaseOrderHandover
    {
        protected readonly Lazy<TblFinOrderHandoverDetailRepository> _detailRepository = new Lazy<TblFinOrderHandoverDetailRepository>();           //订单交接核对明细仓储 

        /// <summary>
        /// 获取已核对的未交接的订单信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-28</para>
        /// </summary>
        /// <param name="handoverDetails">收款交接明细</param>
        /// <param name="tradeType">订单类型</param>
        /// <returns>订单类型的交接核对信息</returns>
        protected virtual OrderHandoverTradeResponse GetOrderHandoverTrade(IEnumerable<TblFinOrderHandoverDetail> handoverDetails, OrderTradeType tradeType)
        {
            //获取招生专员对应已核对的未交接的定金订单信息
            var handoverTradeSummary = handoverDetails.Where(a => a.OrderTradeType == (int)tradeType);
            if (handoverTradeSummary != null && handoverTradeSummary.Any())
            {
                //无现金支付（刷卡/支付宝/微信）
                List<int> payNoCashType = new List<int>()
                {
                    (int)PayType.AliPay,
                    (int)PayType.Pos,
                    (int)PayType.WxPay
                };
                var result = new OrderHandoverTradeResponse()
                {
                    OrderTradeType = (int)tradeType,
                    OrderTradeTypeName = EnumName.GetDescription(typeof(OrderTradeType), tradeType),
                    Cash = handoverTradeSummary.Where(a => a.PayType == (int)PayType.Cash).Sum(b => b.PayAmount),
                    BalanceAmount = handoverTradeSummary.Sum(a => a.UseBalanceAmount),
                    CouponAmount = handoverTradeSummary.Sum(a => a.TotalDiscountFee),
                    HandoverNumber = handoverTradeSummary.Count(),
                    OtherAmount = handoverTradeSummary.Where(a => a.PayType == (int)PayType.Other).Sum(b => b.PayAmount),
                    PayAmount = handoverTradeSummary.Where(a => payNoCashType.Contains(a.PayType)).Sum(b => b.PayAmount),
                    TransferAmount = handoverTradeSummary.Where(a => a.PayType == (int)PayType.BankTransfer).Sum(b => b.PayAmount)
                };
                //合计 = 现钞 + 刷卡 / 微信 / 支付宝 + 转账 + 其他 + 使用余额 + 使用奖学金
                result.TotalAmount = result.Cash + result.BalanceAmount + result.CouponAmount + result.OtherAmount + result.PayAmount + result.TransferAmount;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 将问题单的收款交接更新成已交接
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-20</para>
        /// </summary>
        /// <param name="request">实体请求参数</param>
        /// <param name="handoverDetail">交接明细</param>
        /// <returns>订单交接核对明细Id</returns>
        protected virtual long UpdateUnhandover(OrderHandleAddRequest request, TblFinOrderHandoverDetail handoverDetail)
        {
            handoverDetail.PayDate = request.PayDate;
            handoverDetail.PayType = request.PayType;
            handoverDetail.PayAmount = request.PayAmount.Value;
            handoverDetail.UseBalanceAmount = request.UseBalanceAmount ?? 0;
            handoverDetail.HandoverStatus = (int)HandoverStatus.Checked;
            _detailRepository.Value.Update(handoverDetail);

            return handoverDetail.OrderHandoverDetailId;
        }

        /// <summary>
        /// 数据校验
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="where">表达式</param>
        /// <param name="exceptionId">异常Id</param>
        /// <exception cref="AMS.Core.BussinessException">
        /// 异常ID：exceptionId
        /// </exception>
        protected virtual void CheckData(OrderHandleAddRequest source, Func<OrderHandleAddRequest, bool> where, ushort exceptionId)
        {
            if (source == null)
            {
                return;
            }
            if (where(source))
            {
                throw new BussinessException((byte)ModelType.Order, exceptionId);
            }
        }
    }
}
