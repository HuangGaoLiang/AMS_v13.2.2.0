using System.Collections.Generic;

namespace AMS.Dto
{
    /// <summary>
    /// 教室查看课表数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassRoomCourseTimetableResponse
    {
        /// <summary>
        /// 星期
        /// </summary>
        public string Week { get; set; }

        /// <summary>
        /// 上课时间段
        /// </summary>
        public List<ClassRoomClassTime> ClassTimes { get; set; }
    }

    /// <summary>
    /// 教室上课信息
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-01</para>
    /// </summary>
    public class ClassRoomClassTime
    {
        /// <summary>
        /// 课程简称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 老师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 课程等级中文名称
        /// </summary>
        public string LevelCnName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
