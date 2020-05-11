using AMS.Core;
using JDW.SDK;
using JDW.SDK.Response;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Anticorrosion.JDW
{
    /// <summary>
    /// 描    述：数据仓库
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-01-10</para>
    /// </summary>
    public class JDWService : BService
    {
        /// <summary>
        /// 数据仓库实例化
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        public JDWService()
        {
        }

        #region 公司信息
        /// <summary>
        /// 获取所有公司信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-01-10</para>
        /// </summary>
        /// <returns>公司信息列表</returns>
        public List<CompanyListResponse> GetCompanyList()
        {
            var result = SdkClient.CreateDimCommonService().GetCompanyList();
            return result;
        }
        #endregion

        #region 校区与公司信息
        /// <summary>
        /// 根据校区Id获取校区公司信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-26</para>
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns>校区公司信息</returns>
        public SchoolInfoResponse GetSchoolInfo(string schoolId)
        {
            return GetSchoolInfoList(new List<string> { schoolId }).FirstOrDefault();
        }

        /// <summary>
        /// 根据校区Id集合获取校区公司信息
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-26</para>
        /// </summary>
        /// <returns>校区公司信息列表</returns>
        public List<SchoolInfoResponse> GetSchoolInfoList(List<string> schoolIdList)
        {
            var result = SdkClient.CreateDimCommonService().GetSchoolInfoList(schoolIdList);
            return result;
        }
        #endregion
    }
}
