using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生课程计划
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-28</para>
    /// </summary>
    public class StudyPlanResponse
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 课程学习计划季节信息
        /// </summary>
        public List<StudyPlanTermResponse> Data { get; set; } = new List<StudyPlanTermResponse>();
    }
}
