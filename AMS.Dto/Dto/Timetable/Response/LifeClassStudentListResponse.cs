using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：报班学生课次列表
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-21</para>
    /// </summary>
    public class LifeClassStudentListResponse
    {
        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 已使用课次
        /// </summary>
        public int ClassTimesUse { get; set; }

        /// <summary>
        /// 报班Id
        /// </summary>
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 学生Id
        /// </summary>
        public long StudentId { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long ClassId { get; set; }
    }
}
