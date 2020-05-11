using System;
using System.Collections.Generic;
using System.Linq;
using BDP1.SDK;
using BDP1.SDK.Dto;
using Jerrisoft.Platform.Log;
using YMM.HRS.SDK;
using YMM.HRS.SDK.Dto;

namespace AMS.Anticorrosion.HRS
{
    /// <summary>
    /// 描    述: 获取人事系统数据服务
    /// <para>作    者: Huang GaoLiang </para>
    /// <para>创建时间: 2019-02-18</para>
    /// </summary>
    public class OrgService
    {
        /// <summary>
        /// 根据校区Id获取校区信息
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-21</para>
        /// </summary>
        /// <param name="schoolId">校区Id</param>
        /// <remarks>返回数据可能null值</remarks>
        /// <returns>校区基本信息</returns>
        public static SchoolResponse GetSchoolBySchoolId(string schoolId)
        {
            return new OrgService().GetAllSchoolList().FirstOrDefault(x => x.SchoolId == schoolId);
        }

        #region GetAllSchoolList 获取所有的校区列表

        /// <summary>
        /// 获取所有的校区和城市列表
        /// <para>作     者:Huang GaoLiang  </para>
        /// <para>创建时间: 2019-09-17 </para>
        /// </summary>
        /// <returns>返回校区列表集合</returns>
        public List<SchoolResponse> GetAllSchoolList()
        {
            try
            {
                HrSystem sys = new HrSystem();
                List<SchoolData> schoolDatas = sys.GetSchoolData();
                List<CityResponse> cityList = GetAllCityList();

                List<SchoolResponse> result = (from s in schoolDatas
                                               join c in cityList on s.CityId equals c.CityId into sc
                                               from cc in sc.DefaultIfEmpty()
                                               select new SchoolResponse
                                               {
                                                   CompanyId = s.CompanyId,
                                                   Company = s.CompanyName,
                                                   SchoolId = s.Id,
                                                   SchoolName = s.OrganizationName,
                                                   CityId = s.CityId,
                                                   CityName = cc == null ? "" : cc.CityName
                                               }).ToList();
                return result;
            }
            catch (Exception e)
            {
                LogWriter.Write("GetSchoolData", $"获取所有的校区{e.Message}", LoggerType.Error);
            }
            return new List<SchoolResponse>();
        }
        #endregion

        #region GetAllCityList 获取城市信息
        /// <summary>
        /// 获取城市信息
        /// <para> 作     者:Huang GaoLiang  </para>
        /// <para> 创建时间: 2019-09-17 </para>
        /// </summary>
        /// <returns>返回城市列表集合</returns>
        public List<CityResponse> GetAllCityList()
        {
            try
            {
                HrSystem sys = new HrSystem();
                List<SchoolData> schoolDatas = sys.GetSchoolData();
                var cityIds = schoolDatas.Select(t => t.CityId.ToString()).ToList();

                AreaSdk area = new AreaSdk();
                List<AreaData> areaDatas = area.GetAreaInfos(area.AreaData, cityIds);

                List<CityResponse> cityList = areaDatas.Select(m => new CityResponse
                {
                    CityId = Convert.ToInt32(m.ID),
                    CityName = m.Name
                }).Distinct().ToList();

                cityList = cityList.Where((x, i) => cityList.FindIndex(z => z.CityId == x.CityId) == i).ToList();
                return cityList;
            }
            catch (Exception e)
            {
                LogWriter.Write("GetAreaInfos", $"获取城市信息{e.Message}", LoggerType.Error);
            }

            return new List<CityResponse>();
        }
        #endregion
    }
}
