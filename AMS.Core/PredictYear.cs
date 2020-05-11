using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Core
{
    /// <summary>
    /// 可预见的年
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public static class PredictYear
    {
        /// <summary>
        /// 获取可预见的年
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-28</para>
        /// </summary>
        /// <param name="years">年集合</param>
        /// <returns>年列表</returns>
        public static List<int> Get(List<int> years = null)
        {
            const int totalYear = 5;                             //未来年份长度
            int sYear = DateTime.Now.Year;                       //起始年度
            int eYear = DateTime.Now.AddYears(totalYear).Year;   //结束年度

            if (years != null && years.Any())
            {
                int minYear = years.Min();
                if (minYear < sYear)
                {
                    sYear = minYear;
                }
                int maxYear = years.Max();
                if (maxYear > eYear)
                {
                    eYear = maxYear;
                }
            }

            List<int> res = new List<int>();

            for (int year = sYear; year <= eYear; year++)
            {
                res.Add(year);
            }

            return res;
        }
    }
}
