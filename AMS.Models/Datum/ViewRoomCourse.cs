namespace AMS.Storage.Models
{
    public class ViewRoomCourse
    {
        /// <summary>
        /// 教室课程Id
        /// </summary>
        public long RoomCourseId { get; set; }

        /// <summary>
        /// 教室Id
        /// </summary>
        public long ClassRoomId { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程中文名称
        /// </summary>
        public string CourseCnName { get; set; }

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
        /// 启用/停用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
