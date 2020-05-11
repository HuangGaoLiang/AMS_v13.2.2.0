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
using System;
using System.Collections.Generic;
using AMS.API.Controllers;
using AMS.API.Filter;
using AMS.Core;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Api.skeeper.v1
{
    /// <summary>
    /// 老师课表资源
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-03-08</para>
    /// </summary>
    [Produces("application/json"), Route("api/sKeeper/v1/TeacherLesson")]
    [ApiController]
    [SchoolIdValidator]
    public class TeacherLessonApi : BaseController
    {
        /// <summary>
        /// 获取老师要上课的班级信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-08</para>
        /// </summary>
        /// <param name="classDate">上课日期</param>
        /// <returns>上课的班级信息列表</returns>
        [HttpGet, Route("GetClassLessons")]
        public List<ClassLessonResponse> GetClassLessons(DateTime classDate)
        {
            TeacherTimetableService service =
                new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            return service.GetClassLessons(classDate);
        }

        /// <summary>
        /// 获取老师上课班级下的学生列表
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="lessonType">班级类型</param>
        /// <param name="classDate">上课日期</param>
        /// <returns>班级的学生考勤列表</returns>
        [HttpGet, Route("GetClassStudentLessons")]
        public ClassStudentLessonResponse GetClassStudentLessons(long classId, LessonType lessonType, DateTime classDate)
        {
            TeacherTimetableService service =
                new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            return service.GetClassStudentLessons(classId, lessonType, classDate, base.CurrentUser.UserId);
        }

        /// <summary>
        /// 获取用于补课或调课的老师课程表信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-14</para>
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="classDate">上课时间</param>
        /// <returns>补课或调课的老师课程表信息</returns>
        [HttpGet, Route("GetClassTimetableGeneratorList")]
        public List<ClassTimetableGeneratorResponse> GetClassTimetableGeneratorList(long classId, DateTime classDate)
        {
            TeacherTimetableService service = new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);
            return service.GetClassTimetableGeneratorList(classId, classDate);
        }

        /// <summary>
        /// 获取老师在指定时间段内的上课日期集合
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-11</para>
        /// </summary>
        /// <param name="beginDate">开始时间段</param>
        /// <param name="endDate">结束时间段</param>
        /// <returns>上课日期集合</returns>
        [HttpGet, Route("GetTeacherClassDates")]
        public List<TeacherClassDateResponse> GetTeacherClassDates(DateTime beginDate, DateTime endDate)
        {
            TeacherTimetableService service =
                new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            return service.GetTeacherClassDates(beginDate, endDate);
        }

        /// <summary>
        /// 获取已安排补课或调课的详情
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-12</para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <param name="lessonId">课次ID</param>
        /// <returns>已安排补课或调课的详情</returns>
        [HttpGet, Route("GetStudentReplenishLessons")]
        public StudentReplenishLessonsResponse GetStudentReplenishLessons(long studentId, long lessonId)
        {
            TeacherTimetableService service = new TeacherTimetableService(base.SchoolId, base.CurrentUser.UserId);

            return service.GetStudentReplenishLessons(studentId, lessonId);
        }
    }
}
