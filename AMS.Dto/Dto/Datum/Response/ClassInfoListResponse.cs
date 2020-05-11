using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：班级信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-19</para>
    /// </summary>
    public class ClassInfoListResponse
    {
        /// <summary>
        /// 班级表主键
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassId { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
    }
}
