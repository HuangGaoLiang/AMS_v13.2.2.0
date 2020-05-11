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
using AMS.Dto.Hss;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.API.Api.Hss.v1
{
    /// <summary>
    /// 描    述：学生的相关信息相关资源
    /// <para>作    者：蔡亚康</para>
    /// <para>创建时间：2019-02-26</para>
    /// </summary>
    [Produces("application/json"), Route("api/Hss/v1/Student")]
    [ApiController]
    public class StudentApi : BaseStudentApi
    {

        /// <summary>
        /// 获取家长可以查看到的学生信息列表
        /// 作    者:蔡亚康
        /// 创建时间:2019-02-26
        /// </summary>
        [HttpGet,Route("GetStudents")]
        public List<StudentsResponse> GetStudents()
        {
            FamilyStudentService service = new FamilyStudentService(base.UserPrincipal.UserCode);
            return service.GetStudents();
        }
    }
}
