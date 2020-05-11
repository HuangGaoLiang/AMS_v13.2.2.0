/****************************************************************************\
所属系统:招生系统
所属模块:订单模块
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.Core;
using Jerrisoft.Platform.Public.PageExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：休学订单分页列表数据
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-6</para>
    /// </summary>
    public class LeaveSchoolOrderListResponse : IOrderListResponse
    {
        /// <summary>
        /// 休学人数
        /// </summary>
        public int LeaveSchoolCount { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal LeaveSchoolAmount { get; set; }
        /// <summary>
        /// 休学列表信息
        /// </summary>
        public PageResult<LeaveSchoolOrderInfoResponse> LeaveSchoolOrderList { get; set; }
    }
    /// <summary>
    /// 描述：休学详情信息
    /// <para>作者：瞿琦</para>
    /// <para>创建时间：2019-3-21</para>
    /// </summary>
    public class LeaveSchoolOrderInfoResponse
    {
        /// <summary>
        /// 退费订单表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RefundOrderId { get; set; }
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
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 休学日期
        /// </summary>
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 复课日期
        /// </summary>
        public DateTime ResumeTime { get; set; }
        /// <summary>
        /// 休学课次
        /// </summary>
        public int TotalRefundLessonCount { get; set; }
        /// <summary>
        /// 存入金额（退费金额）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 休学原因
        /// </summary>
        public string Reason { get; set; }
    }

}
