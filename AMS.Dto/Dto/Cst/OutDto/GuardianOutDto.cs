using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描    述: 同步学生至学生联系人表
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-03-06</para>
    /// </summary>
    public class GuardianOutDto
    {
        /// <summary>
        /// 学生编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long StudentId { get; set; }

        /// <summary>
        /// 监护人信息
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string GuardianName { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
