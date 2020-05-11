using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 订单未交接详情
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-16</para>
    /// </summary>
    public class OrderUnHandoverDetailResponse
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long OrderId { get; set; }

        /// <summary>
        /// 收银员Id
        /// </summary>
        public string PersonalId { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        public string PersonalName { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 收款类别
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 收款类别名称
        /// </summary>
        public string OrderTypeName { get; set; }
    }
}
