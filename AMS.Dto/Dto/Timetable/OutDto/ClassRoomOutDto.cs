using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 描述：教室
    /// <para>作    者：Huang GaoLiang</para>
    /// <para>创建时间：2019-03-16</para>
    /// </summary>
    public class ClassRoomOutDto
    {
        /// <summary>
        /// 教室编号
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RoomId { get; set; }

        /// <summary>
        /// 教室名称
        /// </summary>
        public string RoomName { get; set; }
    }
}
