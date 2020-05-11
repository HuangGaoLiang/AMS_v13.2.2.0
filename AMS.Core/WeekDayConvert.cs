using System;

namespace AMS.Core
{
    /// <summary>
    /// 一周的一天转换
    /// <para>作    者：zhiwei.Tang</para>
    /// <para>创建时间：2019-02-28</para>
    /// </summary>
    public static class WeekDayConvert
    {
        /// <summary>
        /// <para>1=周一</para>
        /// <para>2=周二</para>
        /// <para>3=周三</para>
        /// <para>4=周四</para>
        /// <para>5=周五</para>
        /// <para>6=周六</para>
        /// <para>7=周日</para>
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-28</para>
        /// </summary>
        /// <param name="day">天</param>
        /// <returns></returns>
        public static string IntToString(int day)
        {
            string res = string.Empty;

            switch (day)
            {
                case 0:
                    res = "周日";
                    break;
                case 1:
                    res = "周一";
                    break;
                case 2:
                    res = "周二";
                    break;
                case 3:
                    res = "周三";
                    break;
                case 4:
                    res = "周四";
                    break;
                case 5:
                    res = "周五";
                    break;
                case 6:
                    res = "周六";
                    break;
                case 7:
                    res = "周日";
                    break;
            }

            return res;
        }

        /// <summary>
        /// 指定星期几转成对应的字符串
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-28</para>
        /// </summary>
        /// <param name="dayOfWeek">指定星期几。</param>
        /// <returns></returns>
        public static string DayOfWeekToString(DayOfWeek dayOfWeek)
        {
            return IntToString((int)dayOfWeek);
        }

        /// <summary>
        /// 把星期几转换INT
        /// <para>1=星期一</para>
        /// <para>2=星期二</para>
        /// <para>3=星期三</para>
        /// <para>4=星期四</para>
        /// <para>5=星期五</para>
        /// <para>6=星期六</para>
        /// <para>7=星期天</para>
        /// <para>作    者：zhiwei.Tang</para>
        /// <para>创建时间：2019-02-28</para>
        /// </summary>
        /// <param name="time">时间</param>
        public static int DayOfWeekToInt(DateTime time)
        {
            DayOfWeek firstWeek = time.DayOfWeek;

            int week = firstWeek == 0 ? 7 : (int)firstWeek;

            return week;
        }
    }
}
