using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课排课
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-12</para>
    /// </summary>
    public class LifeClassLessonMakeRequest
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 学生报名项Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }
    }
}
