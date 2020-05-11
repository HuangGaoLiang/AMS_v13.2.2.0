using System.Collections.Generic;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 转校订单详情(提交订单前)
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ChangeSchoolOrderTransactDetailResponse : IRefundOrderTransactDetailReponse
    {
        /// <summary>
        /// 转校课程费用明细
        /// </summary>
        public List<TransferCourseFeeDetailResponse> TransferCourseFeeDetail { get; set; }
    }

    /// <summary>
    /// 转校课程费用明细
    /// </summary>
    public class TransferCourseFeeDetailResponse
    {
        /// <summary>
        /// 报名订单课程ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学期类型名称
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程等级名称
        /// </summary>
        public string CourseLevelName { get; set; }

        /// <summary>
        /// 实际单次课时费=实收金额 / 报名课次
        /// </summary>
        public decimal LessonFee { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 已上课次
        /// </summary>
        public int HaveClassLesson { get; set; }

        /// <summary>
        /// 应扣除上课费用
        /// </summary>
        public decimal DeductAmount { get; set; }
    }
}
