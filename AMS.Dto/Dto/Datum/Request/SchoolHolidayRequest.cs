using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 停课日设置
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class SchoolHolidayRequest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间非空")]
        public DateTime STime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间非空")]
        public DateTime ETime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;
    }
}
