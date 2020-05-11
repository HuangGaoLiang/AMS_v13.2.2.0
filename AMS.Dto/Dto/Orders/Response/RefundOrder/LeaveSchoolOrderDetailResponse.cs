using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学订单详情
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class LeaveSchoolOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 休学日期
        /// </summary>
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }
        /// <summary>
        /// 休学原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 补充说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<AttchmentDetailResponse> AttchmentUrl { get; set; }
        /// <summary>
        /// 休学单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 休学费用明细
        /// </summary>
        public List<RefundOrderTransactDetailListResponse> LeaveSchoolOrderDetailList { get; set; }
    }
}
