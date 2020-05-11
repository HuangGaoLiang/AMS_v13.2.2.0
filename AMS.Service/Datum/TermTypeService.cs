using System;
using System.Collections.Generic;
using System.Linq;
using AMS.Dto;
using BDP2.Core.CommonModels;
using BDP2.SDK;

namespace AMS.Service
{
    /// <summary>
    /// 学期类型服务
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-20</para>
    /// </summary>
    public class TermTypeService : BaseDictService
    {
        private static readonly List<string> MultiTermList = new List<string> { "春季班", "秋季班" };

        /// <summary>
        /// 学期类型Key
        /// </summary>
        private const string DICT_CODE = "AMS_TERM_TYPE";

        /// <summary>
        /// 学期类型服务
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-07</para>
        /// </summary>
        public TermTypeService() : base(DICT_CODE) { }

        /// <summary>
        /// 获取所有学期类型
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-09-07</para>
        /// </summary>
        /// <returns>学期类型列表</returns>
        public List<TermTypeResponse> GetAll()
        {
            return new CommonDataSDK<BaseCommonData>().Get(DICT_CODE).Data.OrderBy(m => m.Sort).Select(m => new TermTypeResponse
            {
                TermTypeId = long.Parse(m.Key),
                TermTypeName = m.Name,
                Sort = m.Sort.HasValue ? Convert.ToInt32(m.Sort.Value) : 0,
                MultiTerm = MultiTermList.Any(x => x == m.Name)
            }).ToList();
        }

        /// <summary>
        /// 根据学期类型Id获取学期类型名称
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2018-11-16</para>
        /// </summary>
        /// <param name="termTypeId">学期类型Id</param>
        /// <returns>学期类型名称</returns>
        internal static string GetTermTypeName(long termTypeId)
        {
            return new TermTypeService().GetAll().FirstOrDefault(x => x.TermTypeId == termTypeId)?.TermTypeName;
        }
    }
}
