using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：余额退费详情实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-08</para>
    /// </summary>
    public class BalanceRefundOrderTransactDetailResponse : IRefundOrderTransactDetailReponse
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 监护人
        /// </summary>
        public string GuardianName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 实缴余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
