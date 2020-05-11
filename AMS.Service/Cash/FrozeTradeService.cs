using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 冻结交易
    /// caiyakang 2018-11-22
    /// </summary>
    public class FrozeTradeService : BaseTradeService
    {
        /// <summary>
        /// 实例化一个订单交易的对象
        /// 该订单交易是具体表一个业务订单，并且所属某个校区范围
        /// </summary>
        /// <param name="orderTrade">订单交易接口</param>
        /// <param name="unitOfWork">事务单元</param>
        public FrozeTradeService(IOrderTrade orderTrade, UnitOfWork unitOfWork = null) : base(orderTrade, unitOfWork)
        {
        }



        /// <summary>
        /// 发起交易并使交易进入冻结
        /// </summary>
        internal override void Trade()
        {
            TradeInfo tradeInfo = base._tradeInfo;
            // 1、添加一笔交易记录
            this.AddTradeFroze();

            //2、扣除余额并冻结
            WalletService service = new WalletService(tradeInfo.SchoolId, Convert.ToInt64(tradeInfo.Buyer), base._unitOfWork);
            service.TradeFroze(_orderTrade.TradeType, tradeInfo.OrderId, tradeInfo.TradeBalanceAmount, tradeInfo.Remark);
        }

        /// <summary>
        /// 更新交易完成状态
        /// </summary>
        internal void TradeComplete()
        {
            TradeInfo tradeInfo = base._tradeInfo;
            TblCashOrderTrade entity = _tradeRepository.Value.GetByOrderId(tradeInfo.SchoolId, tradeInfo.OrderId, tradeInfo.OrderNo);
            if (entity.TradeStatus == (int)TradeStatus.Trading)
            {
                entity.TradeStatus = (int)TradeStatus.Complete;
                entity.Remark = _tradeInfo.Remark;
            }
            //1、操作交易完成
            _tradeRepository.Value.Update(entity);

            //2、操作余额冻结完成
            WalletService service = new WalletService(tradeInfo.SchoolId, Convert.ToInt64(tradeInfo.Buyer), base._unitOfWork);
            service.TradeFrozeComplete(_orderTrade.TradeType, tradeInfo.OrderId);
        }

        /// <summary>
        /// 作废交易,从已完成的订单产生一笔作废交易
        /// caiyakang 2018-10-06
        /// </summary>
        internal override void Invalid()
        {
        }

        /// <summary>
        /// 取消交易,从交易中的订单取消
        /// </summary>
        internal override void Cancel()
        {
            TradeInfo tradeInfo = base._tradeInfo;

            //1、处理交易单
            base.UpdateTradeCancel();

            //2、处理余额
            //余额服务不关心订单新增或订单取消,应由交易服务业务来决定如何调余额服务
            if (tradeInfo.TradeBalanceAmount != 0)
            {
                WalletService walltService = new WalletService(tradeInfo.SchoolId, Convert.ToInt64(tradeInfo.Buyer), base._unitOfWork);
                walltService.TradeFrozeCancel(base._orderTrade.TradeType, base.GetCancelTradeType(base._orderTrade.TradeType), tradeInfo.OrderId, tradeInfo.Remark);
            }
        }

        /// <summary>
        /// 添加一笔进入交易冻结的数据
        /// </summary>
        private void AddTradeFroze()
        {
            base.AddTrading();
        }
    }
}
