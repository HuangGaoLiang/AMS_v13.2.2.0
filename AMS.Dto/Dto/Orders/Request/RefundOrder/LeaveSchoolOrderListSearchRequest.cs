using Jerrisoft.Platform.Public.PageExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学查询条件
    /// <para>作   者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class LeaveSchoolOrderListSearchRequest : Page,IOrderListSearchRequest
    {
        /// <summary>
        /// 提交日期 开始时间
        /// </summary>
        public DateTime? CreateStart { get; set; }
        /// <summary>
        /// 提交日期 结束时间
        /// </summary>
        public DateTime? CreateEnd { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
    }
}
