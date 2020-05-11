using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 收款未交接明细请求实体类
    /// </summary>
    public class OrderUnHandoverDetailRequest
    {
        /// <summary>
        /// 所属校区
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 招生专员ID
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int OrderTradeType { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalanceAmount { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }
    }
}
