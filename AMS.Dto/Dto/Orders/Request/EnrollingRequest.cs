using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述:  招生报名订单信息
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class EnrollOrderRequest
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 首次上课时间
        /// </summary>
        public DateTime? FirstClassTime { get; set; }

        /// <summary>
        /// 业绩归属人
        /// </summary>
        public List<AchievementRequest> AchievementList { get; set; }

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
        /// 优惠总额
        /// </summary>
        public decimal TotalDiscountFee { get; set; }

        /// <summary>
        /// 奖学金券总额
        /// </summary>
        public decimal TotalScholarshipFee { get; set; }

        /// <summary>
        /// 使用余额
        /// </summary>
        public decimal UseBalanceAmount { get; set; }

        /// <summary>
        /// 付款总额(实收金额)
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 报名课程明细
        /// </summary>
        public List<EnrollOrderItemRequest> EnrollOrderItem { get; set; }

        /// <summary>
        /// 报名的使用的优惠信息
        /// </summary>
        public List<EnrollDiscountRequest> EnrollDiscounts { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [JsonIgnore]
        public long ParentId { get; set; }

        /// <summary>
        ///转介绍的规则Id
        /// </summary>
        public long? RecommendCouponRuleId { get; set; }

        /// <summary>
        /// 满减规则Id
        /// </summary>
        public long? FullCouponRulId { get; set; }
    }
    /// <summary>
    /// 业绩归属人信息
    /// </summary>
    public class AchievementRequest
    {
        /// <summary>
        /// 业绩归属人
        /// </summary>
        public string PersonalId { get; set; }
        /// <summary>
        /// 业绩比例
        /// </summary>
        public decimal Proportion { get; set; }
    }

    /// <summary>
    /// 报名课程明细
    /// </summary>
    public class EnrollOrderItemRequest
    {
        /// <summary>
        /// 1报名课程 2 报名学期类型
        /// </summary>
        public int EnrollType { get; set; }

        /// <summary>
        /// 报名的学期类型，只允许在该学期类型下的学期报名
        /// </summary>
        public long TermTypeId { get; set; }

        /// <summary>
        /// 报名的年度，只允许排到该年度的学期
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 报名哪个课程,如毕加索
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程类型 （0必修 1选修）
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 报名哪个级别的课程,如初级
        /// </summary>
        public long CourseLevelId { get; set; }

        /// <summary>
        /// 报名的课程是多少分钟的
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 排课课次
        /// </summary>
        public int ClassScheduling { get; set; }

        /// <summary>
        /// 报名的排课信息
        /// </summary>
        public long ClassId { get; set; }
    }

    /// <summary>
    /// 报名的使用的优惠信息
    /// </summary>
    public class EnrollDiscountRequest
    {
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public long CouponId { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal CouponAmount { get; set; }
    }
}
