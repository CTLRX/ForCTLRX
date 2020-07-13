using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Encrypt
{
    public class SignHelper
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="parameters">签名参数</param>
        /// <param name="secret">secret</param>
        /// <param name="isToUpper">返回是否为大写</param>
        /// <param name="split">参数分隔符</param>
        /// <param name="append">参数拼接符</param>
        /// <param name="excludeKeys">需排除的参数</param>
        /// <returns></returns>
        public static string Sign(Dictionary<string, string> parameters, string secret, bool isToUpper = true, string split = "", string append = "", List<string> excludeKeys = null)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && key != "sign")
                {
                    if (excludeKeys != null && !excludeKeys.Contains(key))
                    {
                        query.Append(key).Append(split).Append(value).Append(append);
                    }
                }
            }

            string args = query.ToString();
            if (!string.IsNullOrEmpty(append))
            {
                args = args.Substring(0, args.Length - append.Length);
            }
            args += secret;

            // 第三步：使用MD5加密
            return MD5Encrypt.Encrypt(args, isToUpper);
        }
    }
}
