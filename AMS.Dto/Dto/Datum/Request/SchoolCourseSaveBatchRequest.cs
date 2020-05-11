using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：校区批量授权课程
    /// <para>作    者：Huang GaoLiang </para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public class SchoolCourseSaveBatchRequest
    {
        /// <summary>
        /// 校区编号
        /// </summary>
        public List<string> Schools { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public List<long> Course { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        [JsonIgnore]
        public string CompanyId { get; set; }
    }
}
