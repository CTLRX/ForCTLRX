using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Encrypt
{
    /// <summary>
    /// MD5类型加密
    /// </summary>
    public static class MD5Encrypt
    {
        #region 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="isToUpper">返回值是否为大写</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string source, bool isToUpper = true)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.UTF8.GetBytes(source);
            byte[] md5b = md5.ComputeHash(b);
            md5.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (var item in md5b)
            {
                if (isToUpper)
                {
                    sb.Append(item.ToString("X2"));
                }
                else
                {
                    sb.Append(item.ToString("x2"));
                }
            }
            return sb.ToString();
        }
        #endregion

        #region MD5字符串加密
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="encoding">字符串编码</param>
        /// <param name="isToUpper">返回值是否为大写</param>
        /// <returns></returns>
        public static string Encrypt(string source, Encoding encoding, bool isToUpper = true)
        {
            MD5 md5 = MD5.Create();
            byte[] b = encoding.GetBytes(source);
            byte[] md5b = md5.ComputeHash(b);
            md5.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (var item in md5b)
            {
                if (isToUpper)
                {
                    sb.Append(item.ToString("X2"));
                }
                else
                {
                    sb.Append(item.ToString("x2"));
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
