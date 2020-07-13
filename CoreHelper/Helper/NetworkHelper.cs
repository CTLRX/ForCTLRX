using System.Net;

namespace Omipay.Core
{
    public class NetworkHelper
    {
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public static string GetAddressIP()
        {
            //获取本地的IP地址
            string addressIP = string.Empty;
            foreach( IPAddress ipAddress in Dns.GetHostEntry( Dns.GetHostName() ).AddressList )
            {
                if( ipAddress.AddressFamily.ToString() == "InterNetwork" )
                {
                    addressIP = ipAddress.ToString();
                }
            }
            return addressIP;
        }
    }
}
