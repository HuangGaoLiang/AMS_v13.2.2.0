/****************************************************************************\
所属系统:招生系统
所属模块:客户模块-K信接口
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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AMS.API.Api.skeeper.v1
{
    /// <summary>
    /// 学生资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-27</para>
    /// </summary>
    [Produces("application/json"), Route("api/sKeeper/v1/Student")]
    [ApiController]
    [SchoolIdValidator]
    public class StudentApi : BaseController
    {
        /// <summary>
        /// 根据学生姓名获取本校的学生
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentName">学生名称</param>
        /// <returns>本校学生列表</returns>
        [HttpGet, Route("GetStudents")]
        public List<StudentSearchResponse> GetStudents(string studentName)
        {
            StudentService service = new StudentService(base.SchoolId);

            return service.GetSchoolStudentList(studentName);
        }

        /// <summary>
        /// 根据学生主健获取学生详情信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <returns>学生信息</returns>
        [HttpGet, Route("GetStudent")]
        public StudentDetailResponse GetStudent(long studentId)
        {
            StudentService service = new StudentService(base.SchoolId);

            return service.GetStudent(studentId);
        }
    }
}
