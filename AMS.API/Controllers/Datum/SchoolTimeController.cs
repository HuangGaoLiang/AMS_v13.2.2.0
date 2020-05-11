using AMS.API.Filter;
using AMS.Dto;
using AMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.API.Controllers.Internal
{
    /// <summary>
    /// 描    述: 上课时间段
    /// <para>作    者: Huang GaoLiang</para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>

    [Route("api/AMS/SchoolTime")]
    [ApiController]
    public class SchoolTimeController : BaseController
    {
        /// <summary>
        /// 根据校区编号获取可预见的年份
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07 </para>
        /// </summary>
        /// <returns>返回年份集合</returns>
        [HttpGet, Route("GetPredictYears"), SchoolIdValidator]
        public List<int> GetPredictYears()
        {
            return SchoolTimeService.GetPredictYears(base.SchoolId);
        }

        /// <summary>
        /// 根据学期、和星期几获取上课时间段
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07 </para>
        /// </summary>
        /// <param name="termId">学期主键</param>
        /// <param name="weekDay">星期几</param>
        /// <returns>返回上课时间段集合</returns>
        [HttpGet]
        public List<SchoolTimeDetailResponse> Get(long termId, int weekDay)
        {
            SchoolTimeService service = new SchoolTimeService(termId);
            return service.GetSchoolTimeList(weekDay);
        }

        /// <summary>
        /// 描述，根据日期获取时间段集合
        /// <para>作    者: 瞿琦</para>
        /// <para>创建时间: 2019-3-14</para>
        /// </summary>
        /// <param name="day">日期</param>
        /// <returns>时间段集合</returns>
        [HttpGet,Route("GetDayTimeList"), SchoolIdValidator]
        public List<SchoolDayTimeListResponse> GetDayTimeList(DateTime day)
        {
            var result= SchoolTimeService.GetDayTimeList(base.SchoolId, day);
            return result;
        }


        /// <summary>
        /// 根据主键删除一条上课时间段
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07 </para>
        /// </summary>
        /// <param name="schoolTimeId">上课时间段主键编号</param>
        [HttpDelete]
        public async Task Delete(long schoolTimeId)
        {
            await SchoolTimeService.Remove(schoolTimeId);
        }

        /// <summary>
        /// 保存时间段管理 
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>创建时间: 2018-09-07 </para>
        /// </summary>
        /// <param name="termId">学期编号</param>
        /// <param name="schoolTimeList">上课时间段信息</param>
        [HttpPost, SchoolIdValidator]
        public async Task Post(long termId, [FromBody]List<SchoolTimeSaveRequest> schoolTimeList)
        {
            SchoolTimeService service = new SchoolTimeService(termId);
            await service.Save(schoolTimeList,base.SchoolId);
        }

    }
}
