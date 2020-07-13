using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ConvertHelper
{
   public static  class JsonConvert
    {
        private static JsonSerializerSettings GetConvertConfig(bool isRemoveNullableProperty, bool isCamelCase)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            //忽略循环引用对象
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //忽略json中多出的无法匹配到对象属性的字段
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;
            if (isCamelCase)
            {
                //驼峰命名
                setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (isRemoveNullableProperty)
            {
                //忽略空值
                setting.NullValueHandling = NullValueHandling.Ignore;
            }
            return setting;
        }

        /// <summary>
        /// 将某一类型转换为另一类型
        /// </summary>
        public static T ConvertTo<T>(this object obj)
        {
            return JsonToObject<T>(ObjectToJson(obj));
        }

        /// <summary>
        /// 序列化json到对象
        /// </summary>
        /// <param name="isRemoveNullableProperty">是否忽略空值</param>
        /// <param name="isCamelCase">是否驼峰命名</param>
        public static object JsonToObject(this string jsonString, bool isRemoveNullableProperty = false, bool isCamelCase = false)
        {
            var setting = GetConvertConfig(isRemoveNullableProperty, isCamelCase);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, setting);
        }

        /// <summary>
        /// 序列化json到对象
        /// </summary>
        /// <param name="isRemoveNullableProperty">是否忽略空值</param>
        /// <param name="isCamelCase">是否驼峰命名</param>
        public static T JsonToObject<T>(this string jsonString, bool isRemoveNullableProperty = false, bool isCamelCase = false)
        {
            var setting = GetConvertConfig(isRemoveNullableProperty, isCamelCase);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString, setting);
        }

        /// <summary>
        /// 序列化json到对象
        /// </summary>
        /// <param name="isRemoveNullableProperty">是否忽略空值</param>
        /// <param name="isCamelCase">是否驼峰命名</param>
        public static object JsonToObject(this string jsonString, Type type, bool isRemoveNullableProperty = false, bool isCamelCase = false)
        {
            var setting = GetConvertConfig(isRemoveNullableProperty, isCamelCase);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, type, setting);
        }

        /// <summary>
        /// 序列化对象到json
        /// </summary>
        /// <param name="isRemoveNullableProperty">是否忽略空值</param>
        /// <param name="isCamelCase">是否驼峰命名</param>
        public static string ObjectToJson(this object obj, bool isRemoveNullableProperty = false, bool isCamelCase = false, string dateFormatString = "yyyy-MM-dd HH:mm:ss")
        {
            var setting = GetConvertConfig(isRemoveNullableProperty, isCamelCase);
            setting.DateFormatString = dateFormatString;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, setting);
        }
    }
}
