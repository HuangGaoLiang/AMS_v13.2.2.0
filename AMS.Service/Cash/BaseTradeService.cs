using AMS.Core;
using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Service
{
    public abstract class BaseTradeService
    {

        protected readonly IOrderTrade _orderTrade;  //订单交易接口
        protected readonly TradeInfo _tradeInfo;     //订单交易信息
        protected Lazy<TblCashOrderTradeRepository> _tradeRepository; //交易仓储
        protected readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderTrade"></param>
        /// <param name="unitOfWork">指定该交易的工作单元事务</param>
        protected BaseTradeService(IOrderTrade orderTrade, UnitOfWork unitOfWork = null)
        {            
            this._orderTrade = orderTrade;
            this._tradeInfo = orderTrade.GetTradeInfo();
            this._unitOfWork = unitOfWork;
            //初始化仓储
            InitUnitOfWork();
        }

        /// <summary>
        /// 初始化仓储
        /// </summary>
        private void InitUnitOfWork()
        {
            if (this._unitOfWork != null)
            {
                _tradeRepository = new Lazy<TblCashOrderTradeRepository>(() =>
                {
                    return this._unitOfWork.GetCustomRepository<TblCashOrderTradeRepository, TblCashOrderTrade>();
                });
            }
            else
            {
                _tradeRepository = new Lazy<TblCashOrderTradeRepository>();
            }
        }

        /// <summary>
        /// 当前交易订单数据
        /// </summary>
        private TblCashOrderTrade CurrentOrderTrade
        {
            get
            {
                TradeInfo tradeInfo = this._tradeInfo;
                TblCashOrderTrade entity = _tradeRepository.Value.GetByOrderId(tradeInfo.SchoolId, tradeInfo.OrderId, tradeInfo.OrderNo);
                return entity;
            }
        }



        /// <summary>
        /// 作废交易校验
        /// </summary>
        private void ValidateInvalid(TblCashOrderTrade entity)
        {
            if (entity == null || entity.TradeStatus != (int)TradeStatus.Complete)
            {
                throw new BussinessException(ModelType.Cash, 9);
            }
        }


        /// <summary>
        /// 取消交易校验
        /// </summary>
        private void ValidateCancel(TblCashOrderTrade entity)
        {
            if (entity == null || entity.TradeStatus != (int)TradeStatus.Trading)
            {
                throw new BussinessException(ModelType.Cash, 9);
            }
        }





        /// <summary>
        /// 发起交易并使交易完成
        /// </summary>
        internal abstract void Trade();
        /// <summary>
        /// 作废交易,从已完成的订单产生一笔作废交易
        /// </summary>
        internal abstract void Invalid();
        /// <summary>
        /// 取消交易,从交易中的订单取消
        /// </summary>
        internal abstract void Cancel();





        /// <summary>
        /// 更新交易中的数据
        /// </summary>
        internal void UpdateNewTradingInfo()
        {
            TradeInfo tradeInfo = this._tradeInfo;
            TblCashOrderTrade entity = _tradeRepository.Value.GetByOrderId(tradeInfo.SchoolId, tradeInfo.OrderId, tradeInfo.OrderNo);
            //只有交易中的数据才允许更改
            if (entity.TradeStatus == (int)TradeStatus.Trading)
            {
                entity.TradeAmount = tradeInfo.TradeAmount;
                _tradeRepository.Value.Update(entity);
            }
        }


        /// <summary>
        /// 更新取消交易状态
        /// </summary>
        protected void UpdateTradeCancel()
        {
            TblCashOrderTrade entity = this.CurrentOrderTrade;
            this.ValidateCancel(entity);
            entity.TradeStatus = (int)TradeStatus.Cancel;
            entity.Remark = _tradeInfo.Remark;
            _tradeRepository.Value.Update(entity);
        }


        /// <summary>
        /// 添加一笔进入交易中的数据
        /// </summary>
        protected void AddTrading()
        {
            TradeInfo tradeInfo = this._tradeInfo;
            TblCashOrderTrade entity = this.GetTblCashOrderTrade(tradeInfo);
            entity.TradeType = (int)_orderTrade.TradeType;
            entity.TradeAmount = tradeInfo.TradeAmount;
            entity.TradeBalanceAmount = tradeInfo.TradeBalanceAmount;
            entity.Remark = tradeInfo.Remark;
            entity.TradeStatus = (int)TradeStatus.Trading;
            _tradeRepository.Value.Add(entity);
        }


        /// <summary>
        /// 添加一条作废交易的交易单
        /// caiyakang 2018-11-23
        /// </summary>
        protected void AddTradeInvalid()
        {
            this.ValidateInvalid(this.CurrentOrderTrade);
            TradeInfo tradeInfo = this._tradeInfo;
            TblCashOrderTrade entity = this.GetTblCashOrderTrade(tradeInfo);
            entity.TradeType = (int)GetCancelTradeType(_orderTrade.TradeType);
            entity.TradeAmount = -tradeInfo.TradeAmount;
            entity.TradeBalanceAmount = -tradeInfo.TradeBalanceAmount;
            entity.TradeStatus = (int)TradeStatus.Complete;
            entity.Remark = tradeInfo.Remark;
            entity.CreateTime = DateTime.Now;
            _tradeRepository.Value.Add(entity);
        }

        /// <summary>
        /// 添加一笔进入交易完成的数据
        /// caiyakang 2018-10-06
        /// </summary>
        /// <param name="cancel">是否是订单取消</param>
        protected void AddTradeComplete()
        {
            TradeInfo tradeInfo = this._tradeInfo;
            TblCashOrderTrade entity = GetTblCashOrderTrade(tradeInfo);
            entity.TradeType = (int)_orderTrade.TradeType;
            entity.TradeAmount = tradeInfo.TradeAmount;
            entity.TradeBalanceAmount = tradeInfo.TradeBalanceAmount;
            entity.TradeStatus = (int)TradeStatus.Complete;
            entity.Remark = tradeInfo.Remark;
            _tradeRepository.Value.Add(entity);
        }

        /// <summary>
        /// 统一获取相同的交易单信息
        /// </summary>
        /// <param name="tradeInfo"></param>
        /// <returns></returns>
        private TblCashOrderTrade GetTblCashOrderTrade(TradeInfo tradeInfo)
        {
            TblCashOrderTrade entity = new TblCashOrderTrade();
            entity.OrderTradeId = IdGenerator.NextId();
            entity.SchoolId = tradeInfo.SchoolId;
            entity.OrderId = tradeInfo.OrderId;
            entity.OrderNo = tradeInfo.OrderNo;
            entity.PayType = (int)tradeInfo.PayType;
            entity.TotalDiscount = tradeInfo.TotalDiscount;
            entity.Buyer = tradeInfo.Buyer;
            entity.Seller = tradeInfo.Seller;
            entity.Remark = tradeInfo.Remark;
            entity.CreateTime = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// 交易类型-tradeType代表取消类型
        /// caiyakang 2018-10-06
        /// </summary>
        /// <param name="tradeType"></param>
        /// <returns></returns>
        protected OrderTradeType GetCancelTradeType(OrderTradeType tradeType)
        {
            return (OrderTradeType)(Convert.ToInt32(tradeType) * -1);
        }

    }
}
