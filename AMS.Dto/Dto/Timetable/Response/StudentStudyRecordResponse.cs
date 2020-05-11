using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学习记录
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    public class StudentStudyRecordResponse
    {
        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 学习记录详情
        /// </summary>
        public List<StudentStudyRecordDetailResponse> Data { get; set; } = new List<StudentStudyRecordDetailResponse>();
    }
}
