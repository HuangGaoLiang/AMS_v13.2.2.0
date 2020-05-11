using System;
using System.ComponentModel;
using System.Reflection;

namespace AMS.Dto
{
    public static class EnumName
    {
        private static string GetName(Type t, object v)
        {
            try
            {
                return System.Enum.GetName(t, v);
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
            }
        }
        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(Type t, object v)
        {
            try
            {
                FieldInfo oFieldInfo = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
            }
        }

    }

}
