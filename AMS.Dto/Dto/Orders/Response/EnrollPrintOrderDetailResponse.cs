using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 报名订单详情(打印数据用)
    /// </summary>
    public class EnrollPrintOrderDetailResponse : IOrderDetailResponse
    {
        /// <summary>
        /// 订单详情
        /// </summary>
        public EnrollPrintOrderDetailResponse()
        {

        }


        /// <summary>
        /// 主键(报名订单)
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderId { get; set; }

        /// <summary>
        /// 打印序号
        /// </summary>
        public string PrintNumber { get; set; }

        /// <summary>
        /// 主健(TblCstStudent学生信息)
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
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别名称
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string LinkMobile { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadFaceUrl { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string LinkMail { get; set; }

        /// <summary>
        /// 家庭详细地址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 学生监护人信息
        /// </summary>
        public List<GuardianRequest> ContactPersonList { get; set; }


        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

      
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
        /// 付款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal TotalTradeAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        /// 上课时间
        /// </summary>
        public List<MakeLessonResponse> MakeLessonList { get; set; }
    }

}
