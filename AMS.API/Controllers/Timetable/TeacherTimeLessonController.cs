using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：老师课表资源
    /// <para>作    者：瞿琦</para>
    /// <para>创建时间：2019-3-7</para>
    /// </summary>
    [Route("api/AMS/TeacherTimeLesson")]
    [ApiController]
    public class TeacherTimeLessonController : BsnoController
    {

        /// <summary>
        /// 描述：获取上课的课次信息
        /// 课程信息调整功能 弹窗列表使用
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-7</para>
        /// </summary>
        /// <param name="request">老师上课列表筛选条件</param>
        /// <returns>老师上课的课表信息</returns>
        [HttpGet, Route("GetTeacherClassTimetable")]
        public List<TeacherClassTimetableResponse> GetTeacherClassTimetable([FromQuery]TeacherClassTimetableRequest request)
        {
            var service = new TeacherTimetableService(base.SchoolId, request.TeacherId);
            var teacherTimetableList = service.GetTeacherClassTimetable(request);
            return teacherTimetableList;
        }


        /// <summary>
        /// 描述：获取老师的年度学期班级班级代码数据
        /// <para>作    者：Huang GaoLiang</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <returns>返回老师的年度学期班级班级代码数据</returns>
        [HttpGet, Route("GetTeacherYearTearmClass")]
        public TeacherYearTearmClassResponse GetTeacherYearTearmClass()
        {
            return new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId).GetTeacherYearTearmClass();
        }
    }
}
