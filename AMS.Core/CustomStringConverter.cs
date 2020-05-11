using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMS.Core
{
    public class CustomStringConverter : CustomCreationConverter<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomStringConverter()
        {
        }

        /// <summary>
        /// 重载是否可写
        /// </summary>
        public override bool CanWrite { get { return true; } }

        public override string Create(Type objectType)
        {
            return "";
        }

        /// <summary>
        /// 重载序列化方法
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value.ToString());
            }

        }
    }
}
