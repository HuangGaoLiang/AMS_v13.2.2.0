using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 报名的排课信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class MakeLessonRequest
    {
        /// <summary>
        /// 报名订单明细ID
        /// </summary>
        [Required(ErrorMessage = "不可为空")]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 上课班级
        /// </summary>
        [Required(ErrorMessage = "不可为空")]
        public long ClassId { get; set; }

        /// <summary>
        /// 排多少节课
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 首次上课时间
        /// </summary>
        [Required(ErrorMessage = "不可为空")]
        public DateTime FirstClassTime { get; set; }
    }
}
