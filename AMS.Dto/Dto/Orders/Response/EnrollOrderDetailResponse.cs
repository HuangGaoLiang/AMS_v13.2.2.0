using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  报名订单详情信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 订单详情无参构造函数
        /// </summary>
        public EnrollOrderDetailResponse()
        {

        }

        /// <summary>
        /// 主键(报名订单)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 订单状态编号
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string OrderStatuName { get; set; }

        /// <summary>
        /// 新生/老生
        /// </summary>
        public string OrderNewTypeName { get; set; }
        
        /// <summary>
        /// 年度
        /// </summary>
        public List<OrderYear> YearList { get; set; }

        /// <summary>
        /// 业绩人
        /// </summary>
        public List<EmployeeResponse> PersonalInfo { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalance { get; set; }

        /// <summary>
        /// 付款方式编号
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 付款方式名称
        /// </summary>
        public string PayTypeName { get; set; }

        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal TotalTradeAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 1身份证 2护照 3港澳通行证 4台胞证
        /// </summary>
        public int IDType { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdTypeName { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadFaceUrl { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string LinkMobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string LinkMail { get; set; }

        /// <summary>
        /// 报名总课次
        /// </summary>
        public int TotalClassTimes { get; set; }

        /// <summary>
        /// 学费总额
        /// </summary>
        public decimal TotalTuitionFee { get; set; }

        /// <summary>
        /// 杂费总额
        /// </summary>
        public decimal TotalMaterialFee { get; set; }

        /// <summary>
        /// 赠与奖学金
        /// </summary>
        public decimal Scholarship { get; set; }

        /// <summary>
        /// 奖学金券
        /// </summary>
        public decimal ScholarshipVoucher { get; set; }

        /// <summary>
        /// 付款总额（实收金额）
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 校区编号
        /// </summary>
        public string SchoolId { get; set; }

        /// <summary>
        /// 校区名称
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 打印序号
        /// </summary>
        public string PrintNumber { get; set; }

        /// <summary>
        /// 收款交接
        /// </summary>
        public FinOrderHandoverResponse FinOrderHandoverList { get; set; }

        /// <summary>
        /// 作废操作人
        /// </summary>
        public string CancelUserId { get; set; }

        /// <summary>
        /// 作废操作人名称
        /// </summary>
        public string CancelUserName { get; set; }

        /// <summary>
        /// 作废时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 作废原因
        /// </summary>
        public string CancelRemark { get; set; }

        /// <summary>
        /// 监护人信息
        /// </summary>
        public List<GuardianRequest> ContactPerson { get; set; }
    }

    /// <summary>
    /// 报名课程
    /// </summary>
    public class OrderYear
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 报名订单课程明细
        /// </summary>
        public List<EnrollOrderItemResponse> OrderItemList { get; set; }
    }
}
