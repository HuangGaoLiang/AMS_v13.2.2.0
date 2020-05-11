using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：写生学生班级返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassStudentClassResponse
    {
        /// <summary>
        /// 学生Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 学生班级列表信息
        /// </summary>
        public List<LifeClassClassInfoResponse> StudentClassList { get; set; } = new List<LifeClassClassInfoResponse>();
    }
}
