using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学习记录详情
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-07</para>
    /// </summary>
    public class StudentStudyRecordDetailResponse
    {
        /// <summary>
        /// 报名明细Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long EnrollOrderItemId { get; set; }

        /// <summary>
        /// 学期类型
        /// </summary>
        public string TermTypeName { get; set; }

        /// <summary>
        /// 报名课次
        /// </summary>
        public int ClassTimes { get; set; }

        /// <summary>
        /// 学期
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 上课教室
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 排课课次
        /// </summary>
        public int ArrangedclassTimes { get; set; }

        /// <summary>
        /// 已上课次
        /// </summary>
        public int AttendedClassTimes { get; set; }

        /// <summary>
        /// 剩余课次
        /// </summary>
        public int RemainingClassTimes { get; set; }
    }
}
