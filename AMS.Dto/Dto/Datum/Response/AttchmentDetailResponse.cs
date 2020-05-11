using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 附件信息
    /// </summary>
    public class AttchmentDetailResponse
    {
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
