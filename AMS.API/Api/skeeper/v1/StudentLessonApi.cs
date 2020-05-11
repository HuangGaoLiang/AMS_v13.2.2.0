/****************************************************************************\
所属系统:招生系统
所属模块:课表模块-K信接口
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.API.Controllers;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Jerrisoft.Platform.Log;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Api.skeeper.v1
{
    /// <summary>
    /// 学生考勤资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-27</para>
    /// </summary>
    [Produces("application/json"), Route("api/sKeeper/v1/StudentLesson")]
    [ApiController]
    [SchoolIdValidator]
    public class StudentLessonApi : BaseController
    {
        /// <summary>
        /// 调课,对未上的课次进行调用
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-15</para>
        /// <param name="request">调课,对未上的课次进行调用</param>
        /// </summary>
        [HttpPost, Route("ChangeLesson")]
        public void ChangeLesson(AdjustChangeRequest request)
        {
            var service = new StudentTimetableService(base.SchoolId, request.StudentId);
            service.ChangeLesson(request);
        }

        /// <summary>
        /// 补课,对缺勤或请假的进行补课
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// <param name="request">补课,对缺勤或请假的进行补课</param>
        /// </summary>
        [HttpPost, Route("ReplenishLesson")]
        public void ReplenishLesson(AdjustReplenishRequest request)
        {
            var service = new StudentTimetableService(base.SchoolId, request.StudentId);
            service.ReplenishLesson(request);
        }

        /// <summary>
        /// 描述：考勤补签到
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// <param name="lessonId">课次ID</param>
        /// </summary>
        [HttpPut, Route("SignInAttendReplenish")]
        public void SignInAttendReplenish(long lessonId)
        {
            TeacherTimetableService service = new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            service.SignInAttendReplenish(lessonId);
        }

        /// <summary>
        /// 撤回考勤补签
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// <param name="lessonId">课次ID</param>
        [HttpPut, Route("CancelAttendReplenish")]
        public void CancelAttendReplenish(long lessonId)
        {
            TeacherTimetableService service = new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            service.CancelAttendReplenish(lessonId);
        }

        /// <summary>
        /// 获取缺勤+请假+未上课的课程列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>缺勤+请假+未上课的课程列表</returns>
        [HttpGet, Route("GetStudentAttendanceLessonList")]
        public PageResult<StudentAttendanceLessonSkResponse> GetStudentAttendanceLessonList(
            long studentId, int pageIndex, int pageSize)
        {
            var service = new StudentTimetableService(base.SchoolId, studentId);
            return service.GetStudentAttendanceLessonList(base.CurrentUser.UserId, pageIndex, pageSize);
        }

        /// <summary>
        /// 扫码考勤
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-06</para>
        /// </summary>
        /// <param name="request">老师使用K信扫描学生二维码考勤输入参数</param>
        [HttpPut, Route("ScanCodeAttend")]
        public StudentScanCodeAttendResponse ScanCodeAttend(StudentScanCodeAttendRequest request)
        {
            TeacherTimetableService service = new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            return service.ScanCodeAttend(request);
        }
    }
}
