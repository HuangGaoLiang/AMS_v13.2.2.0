using System;

namespace AMS.Dto
{
    /// <summary>
    /// 学生排课列表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class StudentMakeLessonResponse
    {
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime ClassDate { get; set; }

        /// <summary>
        /// 星期
        /// </summary>
        public string Week { get; set; }

        /// <summary>
        /// 上课时间
        /// </summary>
        public string ClassBeginTime { get; set; }

        /// <summary>
        /// 下课时间
        /// </summary>
        public string ClassEndTime { get; set; }

        /// <summary>
        /// 班级代码
        /// </summary>
        public string ClassNo { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 等级名称
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// 门牌号
        /// </summary>
        public string RoomNo { get; set; }

        /// <summary>
        /// 上课老师
        /// </summary>
        public string TeacherName { get; set; }
    }
}
