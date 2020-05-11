using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学期班级学生实体请求类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class TimeLessonClassRequest
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 学期Id
        /// </summary>
        public long TermId { get; set; }
    }
}
