using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述：学生考勤信息
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2018-11-06</para>
    /// </summary>
    [Route("api/AMS/StudentTimeLesson")]
    [ApiController]
    public class StudentTimeLessonController : BsnoController
    {
        /// <summary>
        /// 获取学生考勤信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-06</para>
        /// </summary>
        /// <param name="studentId">学生Id</param>
        /// <param name="request">学生考勤请求对象</param>
        /// <returns>学生考勤列表</returns>
        [HttpGet, Route("GetLessonStudentList")]
        public List<StudentTimeLessonResponse> GetLessonStudentList(long studentId, [FromQuery] StudentTimeLessonRequest request)
        {
            return new StudentTimetableService(base.SchoolId, studentId).GetLessonStudentList(request);
        }

        /// <summary>
        /// 根据学生Id和学生课程Id获取学生考勤课程详情
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="request">学生考勤详情查询条件</param>
        /// <returns>学生考勤课程详情</returns>
        [HttpGet, Route("GetStudentLessonDetailAsync")]
        public async Task<List<StudentTimeLessonDetailResponse>> GetStudentLessonDetailAsync([FromQuery]StudentTimeLessonDetailRequest request)
        {
            List<long> lessonIdList = request.LessonIdList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select<string, long>(a => Convert.ToInt64(a)).ToList();
            return await new StudentTimetableService(base.SchoolId, request.StudentId).GetStudentLessonDetailAsync(lessonIdList);
        }

        /// <summary>
        /// 获取学生未上课的考勤列表--用于撤销排课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-08 </para>
        /// </summary>
        /// <param name="searcher">撤销排课查询条件</param>
        /// <returns>返回学生未上课的考勤列表</returns>
        [HttpGet, Route("GetStudentNoAttendance")]
        public List<StudentAttendanceLessonResponse> GetStudentNoAttendance([FromQuery]CancelMakeLessonSearchRequest searcher)
        {
            return new StudentTimetableService(base.SchoolId, searcher.StudentId).GetStudentNoAttendance(searcher);
        }

        /// <summary>
        /// 撤销排课
        /// <para>作    者：Huang GaoLiang </para>
        /// <para>创建时间：2019-03-08 </para>
        /// </summary>
        /// <param name="studentId">学生编号</param>
        /// <param name="lessonId">课次编号</param>
        [HttpDelete, Route("CancelMakeLesson")]
        public void CancelMakeLesson(long studentId, long lessonId)
        {
            AdjustRevokeRequest request = new AdjustRevokeRequest
            {
                LessonId = lessonId,
                StudentId = studentId
            };
            new AdjustLessonRevokeService(base.SchoolId).Adjust(request);
        }
    }
}
