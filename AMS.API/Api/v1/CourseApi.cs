using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.API.Controllers;
using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Api.v1
{
    /// <summary>
    /// 课程资源
    /// </summary>
    [Produces("application/json"), Route("api/AMS/Course/v1")]
    [ApiController]
    public class CourseApi : BaseController
    {
        /// <summary>
        /// 获取所有课程信息
        /// caiyakang 2019.01.21
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<CourseListResponse> Get()
        {
            return new CourseService(base.CurrentUser.CompanyId).GetList(null);
        }

    }
}
