using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生余额和奖学金余额
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-14</para>
    /// </summary>
    public class StudentBalanceResponse
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal BalanceAmount { get; set; }
        /// <summary>
        /// 奖学金余额
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// 待转出金额
        /// </summary>
        public decimal TransferAmount { get; set; }
    }
}
