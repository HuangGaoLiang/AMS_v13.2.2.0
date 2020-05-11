using System;

namespace AMS.Core
{
    /// <summary>
    /// 描述：计算你年龄
    /// <para>作    者： Huang GaoLiang </para>
    /// <para>创建时间：2019-02-18 </para>
    /// </summary>
    public static class Age
    {
        /// <summary>
        /// 根据出生年月日计算年龄
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018年11月6日 </para>
        /// </summary>
        /// <param name="dtBirthday">出生年月日</param>
        /// <returns>返回年龄</returns>
        public static string GetAge(DateTime dtBirthday)
        {
            DateTime dtNow = DateTime.Now;
            string strAge = string.Empty; // 年龄的字符串表示
            int intYear = 0; // 岁
            int intMonth = 0; // 月
           
            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1) // 年份输出
            {
                strAge = intYear + "岁";
            }

            if (intMonth > 0) // 五岁以下可以输出月数
            {
                strAge += intMonth + "个月";
            }

           

            if (string.IsNullOrWhiteSpace(strAge))
            {
                strAge = "0岁";
            }

            return strAge;
        }


        /// <summary>
        /// 根据出生年月日计算年龄
        /// <para>作     者:Huang GaoLiang </para>
        /// <para>2018年11月6日 </para>
        /// </summary>
        /// <param name="dtBirthday">出生年月日</param>
        /// <returns>返回年龄</returns>
        public static int GetAgeByBirthday(DateTime dtBirthday)
        {
            DateTime dtNow = DateTime.Now;
            int strAge = 0; // 年龄的字符串表示
            int intYear = 0; // 岁
            int intMonth = 0; // 月
            int intDay = 0; // 天

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);

            }
            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1) // 年份输出
            {
                strAge = intYear;
            }
            return strAge;
        }

        /// <summary>
        /// 根据出生日期与某个时期，计算出年龄（不包括月份）
        /// <para>作    者：郭伟佳</para>
        /// <para>创建时间：2018-11-08</para>
        /// </summary>
        /// <param name="birthday">出生日期</param>
        /// <param name="dateTime">某个时期</param>
        /// <returns>年龄</returns>
        public static int GetAgeByDate(DateTime birthday, DateTime dateTime)
        {
            int age = dateTime.Year - birthday.Year;
            if (dateTime.Month < birthday.Month || (dateTime.Month == birthday.Month && dateTime.Day < birthday.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
    }
}
