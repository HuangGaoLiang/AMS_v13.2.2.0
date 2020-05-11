using System.ComponentModel.DataAnnotations;

namespace AMS.Dto
{
    /// <summary>
    /// 教室列表查询条件
    /// </summary>
    public class ClassListSearchRequest
    {
        /// <summary>
        /// 学期ID
        /// </summary>
        public long TermId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public long CourseId { get; set; }

        /// <summary>
        /// 课程等级编号
        /// </summary>
        public long CourseLeaveId { get; set; }

        /// <summary>
        /// 教室编号
        /// </summary>
        public long RoomId { get; set; }
    }
}
