/*此代码由生成工具字段生成，生成时间2019/3/13 10:52:09 */
using System;

namespace AMS.Storage.Models
{
     /// <summary>
     /// 
     /// </summary>
    public partial class TblDatBusinessConfig
     {
          /// <summary>
          /// 配置的KEY
          /// </summary>
         public string BusinessConfigKey  { get; set; }

          /// <summary>
          /// 所属公司
          /// </summary>
         public string CompanyId  { get; set; }

          /// <summary>
          /// 配置名称
          /// </summary>
         public string BusinessConfigName  { get; set; }

          /// <summary>
          /// 配置数据
          /// </summary>
         public string BusinessConfigData  { get; set; }

          /// <summary>
          /// 创建时间
          /// </summary>
         public DateTime CreateTime  { get; set; }

     }
}
