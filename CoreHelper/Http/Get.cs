using Core.ConvertHelper;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace CoreHelper
{
    public class Get
    {
        private static T GetResult<T>(string returnText, SerializationType type)
        {
            T result = default(T);
            if (type == SerializationType.Json)
            {
                result = JsonConvert.JsonToObject<T>(returnText);

            }
            else
            {
                return XmlConvert.XmlStringToObject<T>(returnText);
            }
            return result;
        }
         
        public static T Request<T>(string url, IDictionary<string, string> formData , SerializationType resultSerializationType = SerializationType.Json, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            string returnText = RequestUtility.HttpGet(url, formData, headers, cookies);
            var result = GetResult<T>(returnText, resultSerializationType);
            return result;
        } 

        public static string Request(string url, IDictionary<string, string> formData , NameValueCollection headers = null, CookieCollection cookies = null) 
        {
            return RequestUtility.HttpGet(url, formData, headers, cookies);
        }

        public static T Request<T>(string url, string data, SerializationType resultSerializationType = SerializationType.Json, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            string returnText = RequestUtility.HttpGet(url, data, headers, cookies);
            var result = GetResult<T>(returnText, resultSerializationType);
            return result;
        }

        public static T Request<T>(string url, string data , NameValueCollection headers = null, CookieCollection cookies = null)
        {
            string returnText = RequestUtility.HttpGet(url, data, headers, cookies);
            var result = GetResult<T>(returnText, SerializationType.Json);
            return result;
        }

        public static string Request(string url, string data , NameValueCollection headers = null, CookieCollection cookies = null)
        {
            return RequestUtility.HttpGet(url, data, headers, cookies);
        }

        public static string Request(string url)
        {
            return RequestUtility.HttpGet(url, "");
        }


    }
}
