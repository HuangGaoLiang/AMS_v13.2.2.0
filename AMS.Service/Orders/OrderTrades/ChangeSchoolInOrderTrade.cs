using System;
using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;

namespace AMS.Service
{
    /// <summary>
    /// 转校订单交易（接收时用）
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-19</para>
    /// </summary>
    internal class ChangeSchoolInOrderTrade : RefundOrderTrade, IOrderTrade
    {
        private readonly TblOdrRefundChangeSchoolOrderRepository _refundChangeSchoolOrderRepository;

        /// <summary>
        /// 根据转校订单对象构建一个转校订单交易对象
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <param name="refundOrder">转校订单对象</param>
        /// <param name="unitOfWork">工作单元</param>
        public ChangeSchoolInOrderTrade(TblOdrRefundOrder refundOrder, UnitOfWork unitOfWork) : base(refundOrder)
        {
            _refundChangeSchoolOrderRepository = 
                unitOfWork.GetCustomRepository<TblOdrRefundChangeSchoolOrderRepository, TblOdrRefundChangeSchoolOrder>();
        }

        public OrderTradeType TradeType => OrderTradeType.ChangeSchoolIn;

        /// <summary>
        /// 转校交易信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <returns>转校交易信息</returns>
        public TradeInfo GetTradeInfo()
        {
            var refundChangeSchoolOrder = _refundChangeSchoolOrderRepository.Load(base.RefundOrder.RefundOrderId);

            return new TradeInfo
            {
                Seller = refundChangeSchoolOrder.InSchoolId,
                OrderId = base.RefundOrder.RefundOrderId,
                OrderNo = base.RefundOrder.OrderNo,
                SchoolId = refundChangeSchoolOrder.InSchoolId,//交易接收方
                Buyer = base.RefundOrder.StudentId.ToString(),
                PayType = PayType.BankTransfer,
                TradeAmount = -(base.RefundOrder.Amount + refundChangeSchoolOrder.TransFromBalance),//实际金额+钱包金额
                TotalDiscount = 0,
                TradeBalanceAmount = base.RefundOrder.Amount + refundChangeSchoolOrder.TransFromBalance, //钱包金额
                Remark = this.GetRemark()
            };
        }

        /// <summary>
        /// 订单交易说明
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-19</para>
        /// </summary>
        /// <returns>订单交易说明</returns>
        private string GetRemark()
        {
            string remark;
            switch ((OrderStatus)base.RefundOrder.OrderStatus)
            {
                case OrderStatus.Paid://转校接收
                    remark = ClientConfigManager.AppsettingsConfig.TransferSchool.Receive;
                    break;
                default:
                    remark = string.Empty;
                    break;
            }
            return remark;
        }

        /// <summary>
        /// 交易失败
        /// </summary>
        /// <param name="tradeFailInfo"></param>
        public void TradeFail(TradeFailInfo tradeFailInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 交易成功
        /// </summary>
        public void TradeSuccess()
        {
            throw new NotImplementedException();
        }
    }
}
