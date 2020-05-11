using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学习课程计划详细信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class StudyPlanTermResponse
    {
        /// <summary>
        /// 季节
        /// </summary>
        public string Season { get; set; }

        /// <summary>
        /// 课程学习计划学期类型相关信息
        /// </summary>
        public List<StudyPlanTermItemResponse> Data { get; set; } = new List<StudyPlanTermItemResponse>();
    }
}
