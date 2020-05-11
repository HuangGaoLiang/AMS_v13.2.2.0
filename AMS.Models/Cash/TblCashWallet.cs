/*此代码由生成工具字段生成，生成时间2018/11/1 15:01:21 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 用户钱包
     /// </summary>
    public partial class TblCashWallet
     {
          /// <summary>
          /// 主健（TblCashWallet）
          /// </summary>
         public long WalletId  { get; set; }

          /// <summary>
          /// 用户ID
          /// </summary>
         public long StudentId  { get; set; }

          /// <summary>
          /// 所属校区
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 余额
          /// </summary>
         public decimal Balance  { get; set; }

          /// <summary>
          /// 冻结资金
          /// </summary>
         public decimal FrozenAmount  { get; set; }

          /// <summary>
          /// 更后更新时间
          /// </summary>
         public DateTime UpdateTime  { get; set; }

     }
}
