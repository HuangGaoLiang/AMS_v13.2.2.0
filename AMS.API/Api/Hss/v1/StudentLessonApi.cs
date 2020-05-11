/****************************************************************************\
所属系统:招生系统
所属模块:家校互联
创建时间:
作   者:
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/
using AMS.API.Api.Hss.Filters;

using AMS.Dto;
using AMS.Dto.Hss;
using AMS.Service;
using Jerrisoft.Platform.Public.PageExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AMS.API.Api.Hss.v1
{
    /// <summary>
    /// 描    述：学生课表的资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/StudentLesson")]
    [ApiController]
    public class StudentLessonApi : BaseStudentApi
    {
        /// <summary>
        /// 获取学生按周、月、学期的课程表--家校互联考勤列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="request">学生课表查询条件</param>
        /// <returns>家校互联学生课表信息</returns>
        [HttpGet, Route("GetStudentLessons")]
        public List<StudentLessonResponse> GetStudentLessons([FromQuery]StudentTimetableSearchRequest request)
        {
            return new StudentTimetableService(request.SchoolId, request.StudentId).GetStudentLessons(request.PeriodType, request.CourseId);
        }

        /// <summary>
        /// 获取学生按周、月、学期的课程表--学期与班级信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-07</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <param name="studentId">学生Id</param>
        /// <returns>学期班级信息</returns>
        [HttpGet, Route("GetStudentClassInfos")]
        public List<StudentTermClassInfoResponse> GetStudentClassInfos(string schoolId, long studentId)
        {
            return new StudentTimetableService(schoolId, studentId).GetStudentClassInfos();
        }

        /// <summary>
        /// 描述:获取可以请假的课次列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">学生上课日期请求条件</param>
        [HttpGet, Route("GetToLeaveLessons")]
        public async Task<List<StudentAttendanceLeaveLessonResponse>> GetToLeaveLessons([FromQuery]StudentAttendanceLeaveLessonRequest request)
        {
            return await new StudentTimetableService(request.SchoolId, request.StudentId).GetStudentNoAttendance(request.ClassDate);
        }

        /// <summary>
        /// 获取当前月及之后的几个月以内的可以请假的课次时间集合
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="request">学生课程请假时间列表请求条件</param>
        /// <returns>上课日期集合</returns>
        [HttpGet, Route("GetToLeaveDates")]
        public List<string> GetToLeaveDates([FromQuery]StudentAttendanceLeaveDatesRequest request)
        {
            return new StudentTimetableService(request.SchoolId, request.StudentId).GetNoAttendanceDates(request.MonthsCount);
        }

        /// <summary>
        /// 描述:学生请假
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="request">学生请假请求条件</param>
        [HttpPut, Route("Leave")]
        public void Leave([FromBody] StudentAttendanceLeaveRequest request)
        {
            new StudentTimetableService(request.SchoolId, request.StudentId).Leave(request.LessonDic);
        }
    }
}
