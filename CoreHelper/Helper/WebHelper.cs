
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
//using System.Web.UI;

namespace Omipay.Core
{
    public class WebHelper
    {
        /// <summary>
        /// 将URL转换为客户端可用的正确URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ResolveUrl(string url)
        {
            if (((HttpContext.Current != null) && (HttpContext.Current.Handler != null)) && (HttpContext.Current.Handler is Page))
            {
                (HttpContext.Current.Handler as Page).ResolveUrl(url);
            }
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            if (url[0] == '~')
            {
                string str;
                url = StripQuery(url, out str);
                return (GenerateClientUrlInternal(HttpContext.Current, url) + str);
            }
            return url;
        }

        private static string GenerateClientUrlInternal(HttpContext httpContext, string contentPath)
        {
            if (contentPath[0] == '~')
            {
                string virtualPath = VirtualPathUtility.ToAbsolute(contentPath, httpContext.Request.ApplicationPath);
                string str2 = httpContext.Response.ApplyAppPathModifier(virtualPath);
                return GenerateClientUrlInternal(httpContext, str2);
            }
            NameValueCollection serverVariables = httpContext.Request.ServerVariables;
            if ((serverVariables == null) || (serverVariables["HTTP_X_ORIGINAL_URL"] == null))
            {
                return contentPath;
            }
            string relativePath = MakeRelative(httpContext.Request.Path, contentPath);
            return MakeAbsolute(httpContext.Request.RawUrl, relativePath);
        }

        private static string MakeRelative(string fromPath, string toPath)
        {
            string str = VirtualPathUtility.MakeRelative(fromPath, toPath);
            if (string.IsNullOrEmpty(str) || (str[0] == '?'))
            {
                str = "./" + str;
            }
            return str;
        }

        private static string MakeAbsolute(string basePath, string relativePath)
        {
            string str;
            basePath = StripQuery(basePath, out str);
            return VirtualPathUtility.Combine(basePath, relativePath);
        }

        private static string StripQuery( string path , out string query )
        {
            int index = path.IndexOf( '?' );
            if( index >= 0 )
            {
                query = path.Substring( index );
                return path.Substring( 0 , index );
            }
            query = null;
            return path;
        }

    }
}
