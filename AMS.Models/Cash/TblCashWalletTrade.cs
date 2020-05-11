/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 用户钱包余额交易流水
     /// </summary>
    public partial class TblCashWalletTrade
     {
          /// <summary>
          /// 主健(TblCashWalletTrade)
          /// </summary>
         public long WalletTradeId  { get; set; }

          /// <summary>
          /// 所属校区
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 用户ID
          /// </summary>
         public long StudentId  { get; set; }

          /// <summary>
          /// 订单交易号
          /// </summary>
         public long OrderId  { get; set; }

          /// <summary>
          /// 交易类型（1 报名订单 2退班退费订单 3游学营退费订单 4余额退费订单）
          /// </summary>
         public int TradeType  { get; set; }

          /// <summary>
          /// 交易金额(可提现)
          /// </summary>
         public decimal TransAmount  { get; set; }

          /// <summary>
          /// 交易前余额
          /// </summary>
         public decimal TransBefBalance  { get; set; }

          /// <summary>
          /// 交易后余额
          /// </summary>
         public decimal TransAftBalance  { get; set; }

          /// <summary>
          /// 交易时间
          /// </summary>
         public DateTime TransDate  { get; set; }

        /// <summary>
        /// 订单交易说明
        /// </summary>
        public string Remark { get; set; }

    }
}
