/****************************************************************************\
所属系统：招生系统
所属模块：公共模块
创建时间：2019-03-13 
作    者：郭伟佳
    * Copyright @ Jerrisoft 2018. All rights reserved.
    *┌─────────────────────────────────── ─┐
    *│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．       │
    *│　版权所有：杰人软件(深圳)有限公司　　　　　　　　　　　                 │
    *└─────────────────────────────────── ─┘
\***************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    /// <summary>
    /// 声明期间类型枚举
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public enum Period { Day, Week, Month, Year };

    /// <summary>
    /// 描    述：日期工具
    /// <para>作    者：郭伟佳</para>
    /// <para>创建时间：2019-03-13</para>
    /// </summary>
    public static class DateUtil
    {
        /// <summary>
        /// 获取当前日期的起止日期
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="period">期间类型</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public static void GetPeriod(Period period, out DateTime beginDate, out DateTime endDate)
        {
            GetPeriod(period, DateTime.Today, out beginDate, out endDate);
        }

        /// <summary>
        /// 获取指定日期的起止日期
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="period">期间类型</param>
        /// <param name="dateTime">指定日期</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public static void GetPeriod(Period period, DateTime dateTime, out DateTime beginDate, out DateTime endDate)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            GetPeriod(period, year, month, out beginDate, out endDate);
        }

        /// <summary>
        /// 获取指定年和月的起止日期
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2019-03-13</para>
        /// </summary>
        /// <param name="period">期间类型</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public static void GetPeriod(Period period, int year, int month, out DateTime beginDate, out DateTime endDate)
        {
            switch (period)
            {
                case Period.Year: //年
                    beginDate = new DateTime(year, 1, 1);
                    endDate = new DateTime(year, 12, 31);
                    break;
                case Period.Month: //月
                    beginDate = new DateTime(year, month, 1);
                    endDate = beginDate.AddMonths(1).AddDays(-1);
                    break;
                case Period.Week: //周
                    int week = (int)DateTime.Today.DayOfWeek;
                    if (week == 0) week = 7; //周日
                    beginDate = DateTime.Today.AddDays(-(week - 1));
                    endDate = beginDate.AddDays(6);
                    break;
                default: //日
                    beginDate = DateTime.Today;
                    endDate = DateTime.Today;
                    break;
            }
        }
    }
}