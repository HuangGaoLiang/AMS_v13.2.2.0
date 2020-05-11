using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学生考勤请求实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    public class StudentTimeLessonRequest
    {
        /// <summary>
        /// 学期Id集合
        /// </summary>
        public List<long> TermIdList { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 状态：1正常-1无效
        /// </summary>
        public LessonUltimateStatus LessonStatus { get; set; } = LessonUltimateStatus.Normal;
    }
}
