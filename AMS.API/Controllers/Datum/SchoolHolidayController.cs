using System.Collections.Generic;
using System.Threading.Tasks;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 停课日资源数据
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2018-09-12</para>
    /// </summary>
    [Route("api/AMS/SchoolHoliday")]
    public class SchoolHolidayController : BsnoController
    {
        /// <summary>
        /// 获取一年的停课日数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <returns>停课日列表</returns>
        [HttpGet]
        public async Task<List<SchoolHolidayResponse>> Get()
        {
            SchoolHolidayService service = new SchoolHolidayService(base.SchoolId, base.Year);
            return await service.GetWeekDayHolidayList();
        }

        /// <summary>
        /// 保存停课日数据
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <param name="dto">停课日设置Dto</param>
        [HttpPost]
        public async Task Post([FromBody]List<SchoolHolidayRequest> dto)
        {
            SchoolHolidayService service = new SchoolHolidayService(base.SchoolId, base.Year);
            await service.Save(dto);
        }

        /// <summary>
        /// 删除一个停课日
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <param name="holidayId">停课日Id</param>
        [HttpDelete]
        public async Task Delete(long holidayId)
        {
            SchoolHolidayService service = new SchoolHolidayService(base.SchoolId, base.Year);
            await service.Remove(holidayId);
        }

        /// <summary>
        /// 获取可预见的年份
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-12</para>
        /// </summary>
        /// <returns>可预见的年份集合</returns>
        [HttpGet, Route("GetPredictYears")]
        public List<int> GetPredictYears()
        {
            return SchoolHolidayService.GetPredictYears(base.SchoolId);
        }
    }
}
