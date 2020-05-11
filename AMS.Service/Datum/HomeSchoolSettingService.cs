/****************************************************************************\
所属系统：招生系统
所属模块：基础资料模块
创建时间：2019-03-05
作   者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/

using AMS.Anticorrosion.HRS;
using AMS.Core.Constants;
using AMS.Dto;
using AMS.Dto.Enum.Custom;
using AMS.Storage.Models;
using AMS.Storage.Repository;
using BDP2.Core.CommonModels;
using BDP2.SDK;
using Jerrisoft.Platform.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Service
{
    /// <summary>
    /// 描    述：家校互联配置业务服务类
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-05</para>
    /// </summary>
    public class HomeSchoolSettingService
    {


        private readonly TblDatBusinessConfigRepository repository;     //家校互联后台设置配置表仓储
        /// <summary>
        /// 描述：实例化一个家校互联配置业务服务类
        /// <para>描    述：瞿琦</para>
        /// <para>创建时间：2019-3-13</para>
        /// </summary>
        public HomeSchoolSettingService()
        {
            repository = new TblDatBusinessConfigRepository();
        }

        #region GetSettingList 根据公司Id获取家校互联的配置列表信息
        /// <summary>
        /// 根据公司Id获取家校互联的配置列表信息
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-11</para>
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>家校互联的配置列表信息</returns>
        public List<HomeSchoolSettingListResponse> GetSettingList(string companyId)
        {
            try
            {

                var companySettingList = new List<HomeSchoolSettingListResponse>();
                //获取家校互联配置信息
                var businessConfig = repository.GetKeyByDatBusinessConfig(HomeSchoolSettingConstants._businessConfigKey);
                if (businessConfig != null)
                {
                    var homeSchoolSettingList = JsonConvert.DeserializeObject<List<HomeSchoolSettingListResponse>>(businessConfig.BusinessConfigData);
                    companySettingList = homeSchoolSettingList.Where(x => x.CompanyId.Trim() == companyId.Trim()).ToList();
                }
                return companySettingList;
            }
            catch (Exception e)
            {
                LogWriter.Write($"根据公司Id获取家校互联的配置列表信息报错{companyId}", $"{e.Message }", LoggerType.Debug);
                throw;
            }
        }
        #endregion

        #region UpdateSetting 更新家校互联配置的值
        /// <summary>
        /// 描述：更新家校互联配置的值
        /// <para>作    者：瞿琦</para>
        /// <para>创建时间：2019-3-13</para>
        /// </summary>
        /// <param name="request">要更新的配置信息</param>
        /// <returns></returns>
        public void UpdateSetting(HomeSchoolSettingListRequest request)
        {
            var businessConfig = repository.GetKeyByDatBusinessConfig(HomeSchoolSettingConstants._businessConfigKey);

            if (businessConfig == null)  //如果没有数据，则添加，反之则修改
            {
                //添加家校互联信息到数据库到数据库
                this.AddHomeSchoolInfo(request);
            }
            else
            {
                this.ModifyHomeSchoolInfo(request, businessConfig);
            }
        }
        #endregion

        #region AddHomeSchoolInfo 添加家校互联数据
        /// <summary>
        /// 描述：添加家校互联数据
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-21</para>
        /// </summary>
        /// <param name="request">要保存的家校互联配置信息</param>
        private void AddHomeSchoolInfo(HomeSchoolSettingListRequest request)
        {
            var homeSchoolResponseList = new List<HomeSchoolSettingListResponse>();
            var homeSchoolResponse = new HomeSchoolSettingListResponse
            {
                CompanyId = request.CompanyId,
                FuntionId = request.FuntionId,
                FuntionName = EnumName.GetDescription(typeof(HomeSchoolBusinessType), request.FuntionId),
                DataId = request.DataId,
                DataValue = request.DataValue
            };
            homeSchoolResponseList.Add(homeSchoolResponse);
            //序列化家校互联设置信息为json字符串
            var businessConfigData = JsonConvert.SerializeObject(homeSchoolResponseList);
            //构建信息保存到数据库
            var tblDatBUsinessConfig = new TblDatBusinessConfig
            {
                BusinessConfigKey = HomeSchoolSettingConstants._businessConfigKey,
                CompanyId = string.Empty,
                BusinessConfigName = HomeSchoolSettingConstants._businessConfigName,
                BusinessConfigData = businessConfigData,
                CreateTime = DateTime.Now
            };
            repository.Add(tblDatBUsinessConfig);
        }
        #endregion

        #region ModifyHomeSchoolInfo 修改家校互联信息
        /// <summary>
        /// 描述：修改家校互联信息
        /// <para>作者：瞿琦</para>
        /// <para>创建时间：2019-3-21</para>
        /// </summary>
        /// <param name="request">要保存的配置信息</param>
        /// <param name="tblDatBusinessConfig">家校互联配置实体</param>
        private void ModifyHomeSchoolInfo(HomeSchoolSettingListRequest request, TblDatBusinessConfig tblDatBusinessConfig)
        {
            //1.反序列化后台配置信息
            var homeSchoolSettingList = JsonConvert.DeserializeObject<List<HomeSchoolSettingListResponse>>(tblDatBusinessConfig.BusinessConfigData);
            //2.判断要修改的功能是否存在
            var changeInfo = homeSchoolSettingList.FirstOrDefault(x => x.CompanyId.Trim() == request.CompanyId.Trim() && x.FuntionId == request.FuntionId);
            if (changeInfo == null)   //如果要修改的功能不存在就添加，反之，则循环修改
            {
                var homeSchoolResponse = new HomeSchoolSettingListResponse
                {
                    CompanyId = request.CompanyId,
                    FuntionId = request.FuntionId,
                    FuntionName = EnumName.GetDescription(typeof(HomeSchoolBusinessType), request.FuntionId),
                    DataId = request.DataId,
                    DataValue = request.DataValue
                };
                homeSchoolSettingList.Add(homeSchoolResponse);
            }
            else
            {
                foreach (var item in homeSchoolSettingList)  //修改要重新配置的家校互联信息
                {
                    if (request.CompanyId == item.CompanyId && item.FuntionId == request.FuntionId)
                    {
                        item.DataId = request.DataId;
                        item.DataValue = request.DataValue;
                    }
                }
            }

            var strBusinessConfigData = JsonConvert.SerializeObject(homeSchoolSettingList);
            tblDatBusinessConfig.BusinessConfigData = strBusinessConfigData;
            repository.Update(tblDatBusinessConfig);
        }
        #endregion

    }
}