using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AMS.Anticorrosion.HRS;
using System.Linq;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 校区、城市资源 REST
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>

    [Route("api/AMS/SchoolCity")]
    [ApiController]
    public class SchoolCityController : BaseController
    {
        /// <summary>
        /// 获取所有校区
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-17</para>
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetSchoolList")]
        public List<SchoolResponse> GetSchoolList()
        {
            //从防腐层中拿数据
            List<SchoolResponse> schoolList = new OrgService().GetAllSchoolList().Where(m => m.CompanyId == base.CurrentUser.CompanyId).ToList();
            return schoolList;
        }
        /// <summary>
        /// 获取所有校区的城市
        /// <para>作    者: Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-17</para>
        /// </summary>
        /// <returns>返回校区城市集合</returns>
        [HttpGet, Route("GetSchoolCitys")]
        public List<CityResponse> GetSchoolCitys()
        {
            return new OrgService().GetAllCityList();
        }
    }
}
