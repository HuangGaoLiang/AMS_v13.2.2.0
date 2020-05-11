using AMS.Core;
using Newtonsoft.Json;

namespace AMS.Dto
{
    /// <summary>
    /// 表示教室的课程数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class RoomCourseResponse
    {
        /// <summary>
        /// 教室Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 教室课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long RoomCourseId { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        [JsonConverter(typeof(CustomStringConverter))]
        public long CourseId { get; set; }

        /// <summary>
        /// 班级中文名称
        /// </summary>
        public string ClassCnName { get; set; }

        /// <summary>
        /// 开班时间段数
        /// </summary>
        public int MaxWeekStage { get; set; }

        /// <summary>
        /// 每个时间段学位数
        /// </summary>
        public int MaxStageStudents { get; set; }

        /// <summary>
        /// 总学位
        /// </summary>
        public int TotalDegree => MaxWeekStage * MaxStageStudents;

        /// <summary>
        /// 启用/停用 false:启用 true:禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
