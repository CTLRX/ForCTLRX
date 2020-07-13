using System.Web;

namespace Omipay.Core
{
    public class UrlHelper
    {
        /// <summary>
        /// 获取当前系统域名
        /// </summary>
        public static string GetDomain()
        {
            if (GetPort() != 80)
            {
                return HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            }
            return HttpContext.Current.Request.Url.Host;
        }

        ///// <summary>
        ///// 获取当前系统域名加虚拟目录（应用程序目录）
        ///// </summary>
        public static string GetDomainAndApplicationPath()
        {
            if (GetPort() != 80)
            {
                return HttpContext.Current.Request.Url.Host + "/" + HttpContext.Current.Request.ApplicationPath + ":" + HttpContext.Current.Request.Url.Port;
            }
            return HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath;
        }

        ///// <summary>
        ///// 获取当前系统端口号
        ///// </summary>
        public static int GetPort()
        {
            return HttpContext.Current.Request.Url.Port;
        }


    }
}
