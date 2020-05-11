using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace AMS.Dto
{
    /// <summary>
    /// 增加一个附件的信息
    /// </summary>
    public class AttchmentAddRequest
    {
        /// <summary>
        /// 附件类型
        /// </summary>
        [JsonIgnore]
        public AttchmentType AttchmentType { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        [JsonIgnore]
        public long BusinessId { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

    }
}
