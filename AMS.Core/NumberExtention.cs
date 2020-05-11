using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 数字扩展类
    /// </summary>
    public static class NumberExtention
    {
        /// <summary>
        /// 数字转中文名
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-09</para>
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns>中文数字</returns>
        public static string GetNumberCnName(this int number)
        {
            switch (number)
            {
                case 0:
                    return "零";
                case 1:
                    return "一";
                case 2:
                    return "两";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                default:
                    return "";
            }
        }
    }
}
