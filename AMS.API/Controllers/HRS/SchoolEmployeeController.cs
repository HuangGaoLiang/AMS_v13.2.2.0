using System.Collections.Generic;
using AMS.Anticorrosion.HRS;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 校区员工
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>

    [Produces("application/json"), Route("api/AMS/SchoolEmployee")]
    [ApiController]
    public class SchoolEmployeeController : BaseController
    {
        /// <summary>
        /// 获取业绩归属人
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-01-11</para>
        /// </summary>
        /// <returns>返回人员信息集合</returns>
        [HttpGet]
        [SchoolIdValidator]
        public List<Anticorrosion.HRS.EmployeeResponse> Get()
        {
            return EmployeeService.GetPerformanceOwnerBySchoolId(base.SchoolId);
        }

        /// <summary>
        /// 描述：获取老师（包含离职N个月的老师）
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-11</para>
        /// </summary>
        /// <param name="dimissionDay">包含离职多少天内的老师</param>
        /// <returns>老师信息列表</returns>
        [HttpGet, Route("GetTeachers")]
        public List<ClassTimetableTeacherResponse> GetTeachers(int dimissionDay)
        {
            var result= TeachService.GetTeachers(base.SchoolId, dimissionDay);
            return result;
        }

        /// <summary>
        /// 获取校区的老师信息列表
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-12-11</para>
        /// </summary>
        /// <returns>老师信息列表</returns>
        [HttpGet, Route("GetTeacherListOfSchool")]
        public List<ClassTimetableTeacherResponse> GetTeacherListOfSchool()
        {
            return TeachService.GetIncumbentTeachers(base.SchoolId);
        }
    }
}
