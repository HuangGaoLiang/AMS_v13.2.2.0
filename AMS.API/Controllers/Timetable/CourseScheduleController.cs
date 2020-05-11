using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 课程表
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-10-29</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/CourseSchedule")]
    [ApiController]
    public class CourseScheduleController : BaseController
    {
        /// <summary>
        /// 获取校区的学期课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <returns>学期课表</returns>
        [HttpGet, Route("GetCourseTimetable")]
        public TermTimetableResponse GetTermCourseTimetable(long termId)
        {
            return new TermCourseSchedule(termId).GetCourseTimetable();
        }

        /// <summary>
        /// 获取学期下的教室课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <param name="classRoomId">教室Id</param>
        /// <returns>教室课表</returns>
        [HttpGet, Route("GetClassRoomCourseTimetable")]
        public async Task<List<ClassRoomCourseTimetableResponse>> GetClassRoomCourseTimetable(long termId, long classRoomId)
        {
            return await new ClassRoomCourseSchedule(classRoomId).GetCourseTimetable(termId, base.CurrentUser.CompanyId);
        }

        /// <summary>
        /// 获取老师课表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-10-29</para>
        /// </summary>
        /// <param name="termId">学期Id</param>
        /// <param name="teacherId">老师Id</param>
        /// <returns>老师课表</returns>
        [HttpGet, Route("GetTeacherCourseTimetable")]
        public async Task<List<TeacherCourseTimetableResponse>> GetTeacherCourseTimetable(long termId, string teacherId)
        {
            return await new TeacherCourseSchedule(termId, teacherId).GetCourseTimetable(base.CurrentUser.CompanyId);
        }
    }
}
