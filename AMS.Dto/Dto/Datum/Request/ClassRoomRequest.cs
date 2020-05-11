using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 教室分配及学位设置
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassRoomRequest
    {
        /// <summary>
        /// 门牌号
        /// </summary>
        [Required(ErrorMessage = "门牌号不可为空")]
        public string RoomNo { get; set; }

        /// <summary>
        /// 教室Id/课程Id
        /// </summary>
        [Required(ErrorMessage = "课程不可为空")]
        public long CourseId { get; set; }

        /// <summary>
        /// 学位数量
        /// </summary>
        [Required(ErrorMessage = "单个时间段学位数不可为空")]
        public int MaxStageStudents { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        [Required(ErrorMessage = "开班时间段数不可为空")]
        public int MaxWeekStage { get; set; }
    }
}
