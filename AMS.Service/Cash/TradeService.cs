using AMS.Dto;
using AMS.Storage;
using AMS.Storage.Repository;
using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Service
{
    /// <summary>
    /// 为所有订单提供普通交易服务
    /// caiyakang 2018-10-06
    /// </summary>
    public class TradeService : BaseTradeService
    {
        /// <summary>
        /// 实例化一个订单交易的对象
        /// 该订单交易是具体表一个业务订单，并且所属某个校区范围
        /// </summary>
        /// <param name="orderTrade">订单交易接口</param>
        public TradeService(IOrderTrade orderTrade, UnitOfWork unitOfWork = null) : base(orderTrade, unitOfWork)
        {
        }

        /// <summary>
        /// 发起交易并使交易完成
        /// </summary>
        internal override void Trade()
        {
            base.AddTradeComplete();//写入交易表
            TradeInfo tradeInfo = base._tradeInfo;
            //余额交易
            if (tradeInfo.TradeBalanceAmount != 0)
            {
                WalletService walltService = new WalletService(tradeInfo.SchoolId, Convert.ToInt64(tradeInfo.Buyer), base._unitOfWork);
                walltService.Trade(this._orderTrade.TradeType, tradeInfo.OrderId, tradeInfo.TradeBalanceAmount, tradeInfo.Remark);
            }
        }


        /// <summary>
        /// 作废交易,从已完成的订单产生一笔作废交易
        /// caiyakang 2018-10-06
        /// </summary>
        internal override void Invalid()
        {
            base.AddTradeInvalid();//写入交易表
            TradeInfo tradeInfo = base._tradeInfo;
            //余额服务不关心订单新增或订单取消,应由交易服务业务来决定如何调余额服务
            if (tradeInfo.TradeBalanceAmount != 0)
            {
                WalletService walltService = new WalletService(tradeInfo.SchoolId, Convert.ToInt64(tradeInfo.Buyer), base._unitOfWork);
                walltService.Trade(GetCancelTradeType(this._orderTrade.TradeType), tradeInfo.OrderId, -tradeInfo.TradeBalanceAmount, tradeInfo.Remark);
            }
        }

        /// <summary>
        /// 取消交易,从交易中的订单取消
        /// </summary>
        internal override void Cancel()
        {

        }



        #region GetOrderTradeList 获取交易记录信息
        /// <summary>
        /// 根据校区Id和查询条件获取交易记录信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="request">交易记录查询条件</param>
        /// <returns>缴费交易记录分页列表</returns>
        public static PageResult<CashOrderTradeListResponse> GetOrderTradeList(string schoolId, CashOrderTradeListRequest request)
        {
            TblCashOrderTradeRepository repository = new TblCashOrderTradeRepository();
            var result = new PageResult<CashOrderTradeListResponse>() { Data = new List<CashOrderTradeListResponse>() };
            //过滤的订单类型
            List<int> ignoreTradeTypeList = new List<int>()
            {
                (int)OrderTradeType.BalanceOrder,
                (int)OrderTradeType.DepositOrder,
                (int)OrderTradeType.CancelBalanceOrder,
                (int)OrderTradeType.CancelDepositOrder,
                (int)OrderTradeType.ChangeSchoolIn
            };
            var tradeList = repository.GetOrderTradeList(schoolId, request, ignoreTradeTypeList);
            if (tradeList != null && tradeList.Data != null && tradeList.Data.Count > 0)
            {
                result.Data = tradeList.Data.Select(a => new CashOrderTradeListResponse()
                {
                    OrderId = a.OrderId,
                    OrderNo = a.OrderNo,
                    TradeAmount = -a.TradeAmount,
                    TradeDate = a.CreateTime,
                    TradeTypeName = EnumName.GetDescription(typeof(OrderTradeType), a.TradeType),
                    TradeType = a.TradeType,
                    Remark = a.Remark
                }).ToList();
                result.CurrentPage = tradeList.CurrentPage;
                result.PageSize = tradeList.PageSize;
                result.TotalData = tradeList.TotalData;
            }
            return result;
        }
        #endregion

    }
}
