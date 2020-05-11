/****************************************************************************\
所属系统：招生系统
所属模块：财务模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Finance
{
    /// <summary>
    /// 描    述：财务考勤核对控制器
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-04</para>
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Attendance")]
    [ApiController]
    public class AttendanceController : BaseController
    {
        /// <summary>
        /// 根据考勤查询条件获取学生考勤核对信息
        /// attendStatus：-1：不显示；0：表示未考勤；1：表示已考勤；2：表示财务已考勤；
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="searchRequest">考勤查询条件</param>
        /// <returns>获取学生考勤核对信息列表</returns>
        [HttpGet, Route("GetStudentAttendanceList")]
        public async Task<StudentAttendCheckResponse> GetStudentAttendanceList([FromQuery]StudentAttendSearchRequest searchRequest)
        {
            return await new AttendanceService(base.SchoolId).GetStudentAttendanceList(searchRequest);
        }

        /// <summary>
        /// 根据考勤查询条件获取学生考勤统计表信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="searchRequest">考勤查询条件</param>
        /// <returns>获取学生考勤统计表</returns>
        [HttpGet, Route("GetStudentAttendanceReport")]
        public async Task<StudentAttendReportResponse> GetStudentAttendanceReport([FromQuery]StudentAttendSearchRequest searchRequest)
        {
            return await new AttendanceService(base.SchoolId).GetStudentAttendanceReport(searchRequest);
        }

        /// <summary>
        /// 更新学生对应的课次考勤状态为已考勤
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-04</para>
        /// </summary>
        /// <param name="lessonStudentId">学生课次Id</param>
        [HttpPut]
        public async Task UpdateAttendStatus([FromBody]List<long> lessonStudentId)
        {
            await new AttendanceService(base.SchoolId).UpdateAttendStatus(lessonStudentId);
        }

        /// <summary>
        /// 老师签字确认
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-15</para>
        /// </summary>
        /// <param name="request">老师签字确认信息请求对象</param>
        [HttpPost, Route("SignConfirmAsync")]
        public async Task SignConfirmAsync(StudentAttendConfirmRequest request)
        {
            await new AttendanceService(base.SchoolId).SignConfirmAsync(request);
        }
    }
}
