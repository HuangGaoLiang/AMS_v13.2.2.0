/*此代码由生成工具字段生成，生成时间17/11/2018 09:43:51 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// TblFinOrderHandover(订单交接核对主表)
     /// </summary>
    public partial class TblFinOrderHandover
     {
          /// <summary>
          /// 主健(订单交接核对主表)
          /// </summary>
         public long OrderHandoverId  { get; set; }

          /// <summary>
          /// 所属校区
          /// </summary>
         public string SchoolId  { get; set; }

          /// <summary>
          /// 招生专员ID
          /// </summary>
         public string PersonalId  { get; set; }

          /// <summary>
          /// 招生专员名称
          /// </summary>
         public string PersonalName  { get; set; }

          /// <summary>
          /// 当日收现钞
          /// </summary>
         public decimal DayIncomeAmout  { get; set; }

          /// <summary>
          /// 存入银行现钞
          /// </summary>
         public decimal InBankAmount  { get; set; }

          /// <summary>
          /// 存单张数
          /// </summary>
         public int ReceiptNumber  { get; set; }

          /// <summary>
          /// （交接人）招生专员签字/日期,图片地址
          /// </summary>
         public string HandoverUrl  { get; set; }

          /// <summary>
          /// （接收人）驻校出纳签字/日期,图片地址
          /// </summary>
         public string RecipientUrl  { get; set; }

          /// <summary>
          /// 差异说明
          /// </summary>
         public string Remark  { get; set; }

          /// <summary>
          /// 交接日期
          /// </summary>
         public DateTime HandoverDate  { get; set; }

          /// <summary>
          /// 创建时间
          /// </summary>
         public DateTime CreateTime  { get; set; }

          /// <summary>
          /// 创建人
          /// </summary>
         public string CreatorId  { get; set; }

          /// <summary>
          /// 创建人名称
          /// </summary>
         public string CreatorName  { get; set; }

     }
}
