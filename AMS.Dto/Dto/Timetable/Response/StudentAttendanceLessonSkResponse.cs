using System;
using System.Collections.Generic;
using System.Text;
using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：学生的考勤课次信息，包括缺勤、请假、未上课
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public class StudentAttendanceLessonSkResponse
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassDate { get; set; }

        /// <summary>
        /// 周几
        /// </summary>
        public string Week { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 上课教师
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 状态 1=请假 2=缺勤 3=未上课
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public string ClassRoom { get; set; }
    }
}
