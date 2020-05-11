using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 班级列表数据查询
    /// </summary>
    public class ClassListResponse
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
        /// 上课时间段（拼接）
        /// </summary>
        public string ClassTime { get; set; }

        /// <summary>
        /// 剩余学位数
        /// </summary>
        public int SurplusNum { get; set; }
    }
}
