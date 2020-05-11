using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生排课结果返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassResultResponse
    {
        /// <summary>
        /// 课次不够学生列表
        /// </summary>
        public List<LifeClassLackTimesListResponse> LackTimeList { get; set; } = new List<LifeClassLackTimesListResponse>();

        /// <summary>
        /// 一个学生多个班级列表
        /// </summary>
        public List<LifeClassStudentClassResponse> StudentClassList { get; set; } = new List<LifeClassStudentClassResponse>();
    }
}
