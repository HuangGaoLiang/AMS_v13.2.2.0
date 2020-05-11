using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class LifeClassAddRequest
    {
        /// <summary>
        /// 学期
        /// </summary>
        [Required]
        public long? TermId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string Title { get; set; }

        /// <summary>
        /// 写生地点
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string Place { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        [Required]
        public string TeacherId { get; set; }

        /// <summary>
        /// 上课开始时间
        /// </summary>
        [Required]
        public DateTime? ClassBeginTime { get; set; }

        /// <summary>
        /// 上课结束时间
        /// </summary>
        [Required]
        public DateTime? ClassEndTime { get; set; }

        /// <summary>
        /// 消耗课次
        /// </summary>
        public int? UseLessonCount { get; set; }
    }
}
