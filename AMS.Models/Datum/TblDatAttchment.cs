using System;
using System.Collections.Generic;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TblDatAttchment
    {
        /// <summary>
        /// 
        /// </summary>
        public long AttchmentId { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }
        /// <summary>
        /// 附件类别
        /// </summary>
        public string AttchmentType { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public long BusinessId { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
