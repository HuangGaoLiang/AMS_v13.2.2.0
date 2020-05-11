using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生课的学生排课数据导出
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassLessonStudentExportResponse
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexName { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdTypeName { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }
    }
}
