using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace CoreHelper
{
    internal static class RequestUtility
    {

        public static string HttpPost(string url, IDictionary<string, string> formData = null, ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            MemoryStream ms = new MemoryStream();
            FillFormDataStream(formData, ms);//填充formData
            return HttpPost(url, ms, contentType, cert, certPassword, headers, cookies);
        }

        public static string HttpPost(string url, string data = null, ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            MemoryStream ms = new MemoryStream();
            FillJsonDataStream(data, ms);//填充formData
            return HttpPost(url, ms, contentType, cert, certPassword, headers, cookies);
        }

        public static string HttpPost(string url, Stream postStream = null, ContentType contentType = ContentType.Json, byte[] cert = null, string certPassword = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 60000;
            request.Method = "POST";
            request.ContentLength = postStream != null ? postStream.Length : 0;
            if (contentType == ContentType.Json)
            {
                request.ContentType = "application/json; charset=UTF-8";
            }
            else if (contentType == ContentType.x_www_form_urlencoded)
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            }
            else if (contentType == ContentType.xml)
            {
                request.ContentType = "text/xml;charset=UTF-8";
            }

            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            if (cert != null)
            {
                if (certPassword != null)
                {
                    X509Certificate2 c = new X509Certificate2(cert, certPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                    request.ClientCertificates.Add(c);
                }
                else
                {
                    X509Certificate2 c = new X509Certificate2(cert);
                    request.ClientCertificates.Add(c);
                }

            }

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(requestStream, Encoding.UTF8);

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(postStream);
                var postStr = sr.ReadToEnd();
                postStream.Close();//关闭
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (cookies != null)
            {
                response.Cookies = request.CookieContainer.GetCookies(response.ResponseUri);
            }

            Stream responseStream = response.GetResponseStream();
            if (response.ContentType.ToLower().IndexOf("gzip") >= 0 || response.ContentEncoding.ToLower().IndexOf("gzip") >= 0)
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            string encodingName = "utf-8";
            if (!string.IsNullOrEmpty(response.CharacterSet))
            {
                encodingName = response.CharacterSet;
            }
            using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding(encodingName)))
            {
                string retString = myStreamReader.ReadToEnd();
                responseStream.Dispose();
                return retString;
            }


        }

        public static void FillFormDataStream(IDictionary<string, string> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        public static void FillJsonDataStream(string data, Stream stream)
        {
            var formDataBytes = data == null ? new byte[0] : Encoding.UTF8.GetBytes(data);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }


        public static string GetQueryString(this IDictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, HttpUtility.UrlEncode(kv.Value));
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }


        public static string HttpGet(string url, IDictionary<string, string> formData = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            string dataString = "";
            if (formData != null)
            {
                dataString = GetQueryString(formData);
            }
            return HttpGet(url, dataString, headers, cookies);
        }

        public static string HttpGet(string url, string dataString = null, NameValueCollection headers = null, CookieCollection cookies = null)
        {
            if (!string.IsNullOrWhiteSpace(dataString))
            {
                if (url.Contains("?"))
                {
                    url += "&";
                }
                else
                {
                    url += "?";
                }
                url += dataString;
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Timeout = 60000;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            if (headers != null)
            {
                request.Headers.Add(headers);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookies != null)
            {
                response.Cookies = request.CookieContainer.GetCookies(response.ResponseUri);
            }

            Stream responseStream = response.GetResponseStream();
            if (response.ContentType.ToLower().IndexOf("gzip") > 0 || response.ContentEncoding.ToLower().IndexOf("gzip") > 0)
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }
            using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
            {
                string retString = myStreamReader.ReadToEnd();
                responseStream.Dispose();
                return retString;
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
            {
                return true;
            }
            return false;
        }
    }
}
