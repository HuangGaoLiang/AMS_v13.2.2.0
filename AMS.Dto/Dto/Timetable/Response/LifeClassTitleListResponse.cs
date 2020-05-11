using AMS.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述：学期写生课返回实体类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-20</para>
    /// </summary>
    public class LifeClassTitleListResponse
    {
        /// <summary>
        /// 写生课Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long LifeTimeId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }
    }
}
