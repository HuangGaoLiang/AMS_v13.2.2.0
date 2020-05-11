using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：转班添加的信息
    /// <para>作   者：瞿琦</para>
    /// <para>创建时间：2018-11-8</para>
    /// </summary>
    public class ChangeClassAddRequest
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        [Required]
        public long StudentId { get; set; }

        /// <summary>
        /// 转出班级
        /// </summary>
        [Required]
        public long OutClassId { get; set; }
        /// <summary>
        /// 转入班级
        /// </summary>
        [Required]
        public long InClassId { get; set; }
        /// <summary>
        /// 开始上课日期
        /// </summary>
        [Required]
        public DateTime InDate { get; set; }
        /// <summary>
        /// 停课日期
        /// </summary>
        [Required]
        public DateTime OutDate { get; set; }

        /// <summary>
        /// 转入课次
        /// </summary>
        public int ClassTimes { get; set; }  
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
