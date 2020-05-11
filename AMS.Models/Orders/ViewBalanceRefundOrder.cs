using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Storage.Models
{
    /// <summary>
    /// 余额退费列表信息
    /// </summary>
    public class ViewBalanceRefundOrder
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 校区Id
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 退费单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string GuardianMobile { get; set; }

        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 扣款金额
        /// </summary>
        public decimal TotalDeductAmount { get; set; }

        /// <summary>
        /// 实退金额
        /// </summary>
        public decimal RealRefundAmount { get; set; }

        /// <summary>
        /// 退费方式
        /// </summary>
        public int RefundType { get; set; }

        /// <summary>
        /// 发卡银行
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// 退费备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 退费日期
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string CreatorName { get; set; }
    }
}
