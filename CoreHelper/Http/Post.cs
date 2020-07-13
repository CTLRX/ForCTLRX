using Core.ConvertHelper;
using CoreHelper;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace CoreHelper
{
    public class Post
    {
        private static T GetResult<T>( string returnText , SerializationType type)
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

        public static T Request<T>( string url , IDictionary<string , string> formData = null ,  SerializationType resultSerializationType = SerializationType.Json,ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null )
        {
            string returnText = RequestUtility.HttpPost( url , formData  , contentType, cert, certPassword, headers, cookies);
            var result = GetResult<T>( returnText , resultSerializationType);
            return result;
        } 

        public static string Request( string url , IDictionary<string , string> formData = null ,  ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null )
        {
            string returnText = RequestUtility.HttpPost( url , formData  , contentType, cert, certPassword, headers, cookies);
            return returnText;
        }

        public static T Request<T>( string url , string data = null , SerializationType resultSerializationType = SerializationType.Json, ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null )
        {
            string returnText = RequestUtility.HttpPost( url , data , contentType, cert, certPassword, headers, cookies);
            var result = GetResult<T>( returnText , resultSerializationType);
            return result;
        }

        public static T Request<T>(string url, string data = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            string returnText = RequestUtility.HttpPost(url, data, ContentType.Json, null, null, headers, cookies);
            var result = GetResult<T>(returnText, SerializationType.Json);
            return result;
        }

        public static string Request( string url , string data = null , ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null )
        {
            string returnText = RequestUtility.HttpPost( url , data , contentType, cert, certPassword, headers, cookies);
            return returnText;
        }

    }
}
