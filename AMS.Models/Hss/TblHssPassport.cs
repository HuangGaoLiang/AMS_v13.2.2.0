/*此代码由生成工具字段生成，生成时间2019/3/6 16:11:58 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 
     /// </summary>
    public partial class TblHssPassport
     {
          /// <summary>
          /// 
          /// </summary>
         public long PassporId  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public string UserCode  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public string OpenId  { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string UnionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrentLoginIp  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public DateTime? CurrentLoginDate  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public string LastLoginIp  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public DateTime? LastLoginDate  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public int LoginTimes  { get; set; }

          /// <summary>
          /// 
          /// </summary>
         public DateTime CreateTime  { get; set; }

     }
}
