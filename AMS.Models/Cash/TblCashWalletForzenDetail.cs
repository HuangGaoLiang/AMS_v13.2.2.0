/*此代码由生成工具字段生成，生成时间14/11/2018 20:16:09 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 用户钱包余额冻结明细
     /// </summary>
    public partial class TblCashWalletForzenDetail
     {
          /// <summary>
          /// 主键（TblCashWalletForzenDetail）
          /// </summary>
         public long WalletForzenDetailId  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public long StudentId  { get; set; }

          /// <summary>
          /// 业务类型
          /// </summary>
         public int BusinessType  { get; set; }

          /// <summary>
          /// 业务ID
          /// </summary>
         public long BusinessId  { get; set; }

          /// <summary>
          /// 金额
          /// </summary>
         public decimal Amount  { get; set; }

          /// <summary>
          /// 状态（-1作废1处理中2已完成）
          /// </summary>
         public int Status  { get; set; }

          /// <summary>
          /// 创建时间
          /// </summary>
         public DateTime CreateTime  { get; set; }

     }
}
